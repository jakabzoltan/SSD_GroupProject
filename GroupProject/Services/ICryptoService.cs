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
 * Date: 2018-11-4
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupProject.Services
{
	/// <summary>
	/// Represents a cryptography service.
	/// </summary>
	public interface ICryptoService
	{
		/// <summary>
		/// Decrypts the content.
		/// </summary>
		/// <param name="encryptedContent">Content of the encrypted.</param>
		/// <returns>Returns the decrypted content.</returns>
		string DecryptContent(string encryptedContent);

		/// <summary>
		/// Encrypts the content.
		/// </summary>
		/// <param name="content">The content.</param>
		/// <returns>Returns the encrypted content.</returns>
		string EncryptContent(string content);

		/// <summary>
		/// Signs the content.
		/// </summary>
		/// <param name="encryptedContent">The encrypted content.</param>
		/// <returns>Returns the signed content.</returns>
		string SignContent(byte[] encryptedContent);

		/// <summary>
		/// Verifies the signed content.
		/// </summary>
		/// <param name="signedContent">The signed content.</param>
		/// <returns><c>true</c> if the computed hash and the stored hash match, <c>false</c> otherwise.</returns>
		bool VerifySignedContent(string signedContent);
	}
}
