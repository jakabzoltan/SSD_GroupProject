/*
 * Copyright 2016-2018 Mohawk College of Applied Arts and Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: nitya
 * Date: 2018-11-5
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace GroupProject.Services
{
	/// <summary>
	/// Represents a cryptography service.
	/// </summary>
	/// <seealso cref="CryptographyExample.Services.ICryptoService" />
	public class CryptoService : ICryptoService
	{
		/// <summary>
		/// The configuration.
		/// </summary>
		private readonly IConfiguration configuration;

		/// <summary>
		/// The AES key.
		/// </summary>
		private readonly string aesKey;

		/// <summary>
		/// The AES IV.
		/// </summary>
		private readonly string aesIv;

		/// <summary>
		/// The asymmetric private key.
		/// </summary>
		private readonly string asymmetricPrivateKey;

		/// <summary>
		/// The HMAC key.
		/// </summary>
		private readonly string hmacKey;

		/// <summary>
		/// The use asymmetric encryption.
		/// </summary>
		private readonly bool useAsymmetricEncryption;

		/// <summary>
		/// Initializes a new instance of the <see cref="CreditCardService" /> class.
		/// </summary>
		public CryptoService(IConfiguration configuration)
		{
			this.configuration = configuration;
			this.aesKey = this.configuration.GetValue<string>("AesKey");
			this.aesIv = this.configuration.GetValue<string>("AesIv");
			this.hmacKey = this.configuration.GetValue<string>("HmacKey");
			this.useAsymmetricEncryption = this.configuration.GetValue<bool>("UseAsymmetricEncryption");
			this.asymmetricPrivateKey = this.configuration.GetValue<string>("AsymmetricPrivateKey");
		}

		/// <summary>
		/// Decrypts the content.
		/// </summary>
		/// <param name="encryptedContent">Content of the encrypted.</param>
		/// <returns>Returns the decrypted content.</returns>
		public string DecryptContent(string encryptedContent)
		{
			if (encryptedContent == null)
			{
				throw new ArgumentNullException(nameof(encryptedContent), "Value cannot be null");
			}

			string result;

			if (!this.useAsymmetricEncryption)
			{
				// create an instance of the AES encryption algorithm
				using (var aes = Aes.Create())
				{
					// supply the key 
					aes.Key = Convert.FromBase64String(this.aesKey);

					// supply the initialization vector
					aes.IV = Convert.FromBase64String(this.aesIv);

					// create the decryptor instance which will be used to decrypt our data
					var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

					// convert the content from a base 64 encoded string
					// to a byte array
					var input = Convert.FromBase64String(encryptedContent);

					// decrypt the content
					// input - encrypted content
					// 0 - represents the offset value to indicate where to start the decryption process
					// input.Length - represents the the length of content to be decrypted
					var decryptedContent = decryptor.TransformFinalBlock(input, 0, input.Length);

					// convert the result back to a human readable string
					result = Encoding.UTF8.GetString(decryptedContent);
				}
			}
			else
			{
				// create the RSA crypto service provider
				using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider())
				{
					// import the private key
					rsaCryptoServiceProvider.ImportCspBlob(Convert.FromBase64String(asymmetricPrivateKey));

					// convert the encrypted data from a base 64 encoded string
					var dataToEncrypt = Convert.FromBase64String(encryptedContent);

					// decrypt the data
					var decryptedData = rsaCryptoServiceProvider.Decrypt(dataToEncrypt, true);

					// convert the decrypted byte array back to the original string
					result = Encoding.UTF8.GetString(decryptedData);
				}
			}

			return result;
		}

		/// <summary>
		/// Encrypts the content.
		/// </summary>
		/// <param name="content">The content.</param>
		/// <returns>Returns the encrypted content.</returns>
		public string EncryptContent(string content)
		{
			if (string.IsNullOrEmpty(content))
			{
				throw new ArgumentNullException(nameof(content), "Value cannot be null");
			}

			string result;

			if (!this.useAsymmetricEncryption)
			{
				// this is symmetric encryption
				// create a new instance of the AES class
				using (var aes = Aes.Create())
				{
					// the key to use to encrypt the data
					aes.Key = Convert.FromBase64String(this.aesKey);

					// the IV to use to as the start of the encrypted data 
					aes.IV = Convert.FromBase64String(this.aesIv);

					// create the encryptor instance
					// using the AES algorithm
					// supply the key and initialization vector
					// to create the encryptor instance
					var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

					// get the UTF-8 bytes of our content to encrypt
					// use the encoding class, with the UTF-8 encoding
					// to retrieve the bytes copy of the original content
					var input = Encoding.UTF8.GetBytes(content);

					// [[IV, some random data]]
					// apply the key and the encryption algorithm
					// use the encryptor to encrypt our data
					// input is the data to encrypt
					// 0 represents the offset of the byte array
					// input.Length represents the length of the byte array content
					// to be encrypted
					var encryptedContent = encryptor.TransformFinalBlock(input, 0, input.Length);

					// convert the encrypted content to a base 64 encoded string
					result = Convert.ToBase64String(encryptedContent);
				}
			}
			else
			{
				// create the RSA crypto service provider
				using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider())
				{
					// import the public private key pair
					rsaCryptoServiceProvider.ImportCspBlob(Convert.FromBase64String(asymmetricPrivateKey));

					// get the byte array content of the data to encrypt
					// using the UTF-8 encoding
					var dataToEncrypt = Encoding.UTF8.GetBytes(content);

					// encrypt the data
					// include padding during the encryption process, to add to the randomness of the encrypted data
					var encryptedData = rsaCryptoServiceProvider.Encrypt(dataToEncrypt, true);

					// convert the encrypted content to a base 64 encoded string
					result = Convert.ToBase64String(encryptedData);
				}
			}

			return result;
		}

		/// <summary>
		/// Signs the content.
		/// </summary>
		/// <param name="encryptedContent">The encrypted content.</param>
		/// <returns>Returns the signed content.</returns>
		public string SignContent(byte[] encryptedContent)
		{
			string content;

			// Initialize the keyed hash object
			using (var hmac = new HMACSHA512(Convert.FromBase64String(this.hmacKey)))
			{
				// read in the encrypted content
				var inStream = new MemoryStream(encryptedContent);

				// compute the hash value of the encrypted content
				var hashValue = hmac.ComputeHash(inStream);

				// create an out stream to write out signed content to
				var outStream = new MemoryStream();

				// MUST RE-POSITION the stream
				inStream.Position = 0;

				// write the computed hash value to the output stream
				outStream.Write(hashValue, 0, hashValue.Length);

				int bytesRead;

				// read 1K at a time
				var buffer = new byte[1024];

				do
				{
					// Read from the wrapping CryptoStream
					// read 1K at a time until we get to the end of the stream
					bytesRead = inStream.Read(buffer, 0, 1024);

					// write the output to the stream
					outStream.Write(buffer, 0, bytesRead);
				} while (bytesRead > 0);

				// convert the output stream to a Base64 encoded string
				content = Convert.ToBase64String(outStream.ToArray());
			}

			if (!this.VerifySignedContent(content))
			{
				throw new InvalidOperationException("Unable to verify HMAC signature");
			}

			return content;
		}

		/// <summary>
		/// Verifies the signed content.
		/// </summary>
		/// <param name="signedContent">The signed content.</param>
		/// <returns><c>true</c> if the computed hash and the stored hash match, <c>false</c> otherwise.</returns>
		public bool VerifySignedContent(string signedContent)
		{
			var error = false;
			var status = false;

			var sourceContent = Convert.FromBase64String(signedContent);

			// Initialize the keyed hash object
			using (var hmac = new HMACSHA512(Convert.FromBase64String(this.hmacKey)))
			using (var inStream = new MemoryStream(sourceContent))
			{
				// Create an array to hold the keyed hash value read from the content.
				// the Hash size is 512 bytes here
				// divide that by 8 to ensure we have a correct number of bytes
				var storedHash = new byte[hmac.HashSize / 8];

				// Read in the storedHash
				inStream.Read(storedHash, 0, storedHash.Length);

				// Compute the hash of the remaining contents.
				// The stream is properly positioned at the beginning of the content,
				// immediately after the stored hash value.
				var computedHash = hmac.ComputeHash(inStream);

				// compare the computed hash with the stored value
				// for each byte in each array of the stored has
				// and the computed hash, we want to compare each byte
				// to ensure that the signed message has not been
				// tampered with during transport
				for (var i = 0; i < storedHash.Length; i++)
				{
					if (computedHash[i] != storedHash[i])
					{
						error = true;
					}
				}
			}

			if (error)
			{
				Trace.TraceWarning("Hash values differ! Signed content has been tampered with!");
			}
			else
			{
				Trace.TraceWarning("Hash values are equal -- no tampering occurred.");
				status = true;
			}

			return status;
		}
	}
}
