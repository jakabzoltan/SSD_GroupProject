using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupProject.Models;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using GroupProject.Services;
using GroupProject.Data;

namespace GroupProject.Controllers
{
    /// <summary>
    /// I Zoltan Jakab, 000373180, certify that this material is my original work.
    /// No other person's work has been used without due acknowledgement 
    /// and I have not made my work available to anyone else
    /// 
    /// Acknowlegment to: Nityan Khanna
    ///     - For providing Keygen and Crypto Services for encrypting, signing and verifying content.
    /// </summary>
    public class CreditCardsController : Controller
    {
        private readonly GroupProjectContext _context;
        private readonly ICryptoService _cryptoService;
        public CreditCardsController(GroupProjectContext context, ICryptoService cryptoService)
        {
            _context = context;
            _cryptoService = cryptoService;
        }

        // GET: CreditCards
        public async Task<IActionResult> Index()
        {
            return View(await _context.CreditCard.ToListAsync());
        }

        // GET: CreditCards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creditCard = await _context.CreditCard
                .SingleOrDefaultAsync(m => m.Id == id);
            if (creditCard == null)
            {
                return NotFound();
            }


            if (_cryptoService.VerifySignedContent(creditCard.SECC))
            {
                creditCard.PTCC = _cryptoService.DecryptContent(creditCard.ECC);
            } else
            {
                creditCard.PTCC = "Signature verification failed";
            }

            return View(creditCard);
        }

        // GET: CreditCards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CreditCards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PTCC")] CreditCard creditCard)
        {
            if (ModelState.IsValid)
            {
                creditCard.ECC = _cryptoService.EncryptContent(creditCard.PTCC);
                creditCard.SECC = _cryptoService.SignContent(Convert.FromBase64String(creditCard.ECC));
                _context.Add(creditCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(creditCard);
        }

        // GET: CreditCards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creditCard = await _context.CreditCard.SingleOrDefaultAsync(m => m.Id == id);
            if (creditCard == null)
            {
                return NotFound();
            }


            if (_cryptoService.VerifySignedContent(creditCard.SECC))
            {
                creditCard.PTCC = _cryptoService.DecryptContent(creditCard.ECC);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }


            return View(creditCard);
        }

        // POST: CreditCards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ECC,SECC")] CreditCard creditCard)
        {
            if (id != creditCard.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(creditCard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CreditCardExists(creditCard.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(creditCard);
        }

        // GET: CreditCards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creditCard = await _context.CreditCard
                .SingleOrDefaultAsync(m => m.Id == id);
            if (creditCard == null)
            {
                return NotFound();
            }

            return View(creditCard);
        }

        // POST: CreditCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var creditCard = await _context.CreditCard.SingleOrDefaultAsync(m => m.Id == id);
            _context.CreditCard.Remove(creditCard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CreditCardExists(int id)
        {
            return _context.CreditCard.Any(e => e.Id == id);
        }

        public IActionResult GenerateKey()
        {
            var model = new KeygenModel();

            var hmacKey = new byte[64];

            using (var rngProvider = new RNGCryptoServiceProvider())
            {
                rngProvider.GetBytes(hmacKey);
            }

            model.HmacKey = Convert.ToBase64String(hmacKey);

            // creates a new key and IV every time
            using (var aes = Aes.Create())
            {
                model.AesKey = Convert.ToBase64String(aes.Key);
                model.AesIv = Convert.ToBase64String(aes.IV);
            }

            using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider())
            {
                // export the parameters (private key)
                var parameterBlob = rsaCryptoServiceProvider.ExportCspBlob(true);

                model.AsymmetricPrivateKey = Convert.ToBase64String(parameterBlob);
            }

            return View(model);
        }
    }
}
