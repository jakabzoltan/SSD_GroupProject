using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using GroupProject.Models;

namespace GroupProject.Services
{
    /// <summary>
    /// Represents a credit card service.
    /// </summary>
    public interface ICreditCardService
    {
        /// <summary>
        /// Creates the credit card asynchronously.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <param name="creditCard">The credit card.</param>
        /// <param name="cvcCode">The CVC code.</param>
        /// <returns>Returns the id of the created credit card.</returns>
        Task<CreditCard> CreateCreditCardAsync(string creditCard, string cvcCode, string userId);

        /// <summary>
        /// Gets the credit card asynchronously.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns the credit card or null if no credit card record is  found.</returns>
        Task<CreditCard> GetCreditCardAsync(Guid id);

        /// <summary>
        /// Finds a list of credit cards which match the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="count">The count.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>Returns a list of credit cards which match the specified expression.</returns>
        Task<IEnumerable<CreditCard>> QueryAsync(Expression<Func<CreditCard, bool>> expression, int? count, int offset);

        Task<bool> UpdateCreditCard(CreditCard card);
    }
}