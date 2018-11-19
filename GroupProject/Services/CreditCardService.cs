
using GroupProject.Data;
using GroupProject.Models;
using GroupProject.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.Services
{
    /// <summary>
    /// Represents a cryptography service.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="CryptographyExample.Services.ICreditCardService" />
    public class CreditCardService : ICreditCardService, IDisposable
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly GroupProjectContext context;

        /// <summary>
        /// The crypto service.
        /// </summary>
        private readonly ICryptoService cryptoService;

        /// <summary>
        /// The data protector.
        /// </summary>
        private readonly IDataProtector dataProtector;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditCardService" /> class.
        /// </summary>
        /// <param name="cryptoService">The crypto service.</param>
        /// <param name="context">The context.</param>
        /// <param name="dataProtectionProvider">The data protection provider.</param>
        public CreditCardService(ICryptoService cryptoService, GroupProjectContext context, IDataProtectionProvider dataProtectionProvider)
        {
            this.context = context;
            this.dataProtector = dataProtectionProvider.CreateProtector("CvcCodeProtector");
            this.cryptoService = cryptoService;
        }

        /// <summary>
        /// Creates the credit card asynchronously.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <param name="creditCard">The credit card.</param>
        /// <param name="cvcCode">The CVC code.</param>
        /// <returns>Returns the id of the created credit card.</returns>
        public async Task<CreditCard> CreateCreditCardAsync(string creditCard, string cvcCode)
        {
            var encryptedContent = this.cryptoService.EncryptContent(creditCard);
            var signedContent = this.cryptoService.SignContent(Convert.FromBase64String(encryptedContent));

            var cc = new CreditCard()
            {
                CvcCode = dataProtector.Protect(cvcCode),
                ECC = encryptedContent,
                SECC = signedContent
            };

            this.context.CreditCard.Add(cc);
            await this.context.SaveChangesAsync();

            return cc;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.context?.Dispose();
        }

        /// <summary>
        /// Gets the credit card asynchronously.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns the credit card or null if no credit card record is  found.</returns>
        public async Task<CreditCard> GetCreditCardAsync(Guid id)
        {
            var creditCard = await this.context.CreditCard.FindAsync(id);

            if (creditCard == null)
            {
                throw new KeyNotFoundException($"Unable to find credit card using id: {id}");
            }

            creditCard.CvcCode = this.dataProtector.Unprotect(creditCard.CvcCode);

            return creditCard;
        }

        /// <summary>
        /// Finds a list of credit cards which match the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="count">The count.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>Returns a list of credit cards which match the specified expression.</returns>
        public async Task<IEnumerable<CreditCard>> QueryAsync(Expression<Func<CreditCard, bool>> expression, int? count, int offset)
        {
            var creditCards = this.context.CreditCard.Where(expression);

            if (offset > 0)
            {
                creditCards = creditCards.Skip(offset);
            }

            creditCards = creditCards.Take(count ?? 25);

            foreach (var creditCard in creditCards)
            {
                creditCard.CvcCode = this.dataProtector.Unprotect(creditCard.CvcCode);
            }

            return await creditCards.ToListAsync();
        }
    }
}