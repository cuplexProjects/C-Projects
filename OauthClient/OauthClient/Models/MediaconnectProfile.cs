using System.Collections.Generic;

namespace OauthClient.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Credential
    {
        /// <summary>
        /// Gets or sets the credential.
        /// </summary>
        /// <value>
        /// The credential.
        /// </value>
        public string credential { get; set; }
        /// <summary>
        /// Gets or sets the type of the credential.
        /// </summary>
        /// <value>
        /// The type of the credential.
        /// </value>
        public string credentialType { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LoginCredential
    {
        /// <summary>
        /// Gets or sets the credential.
        /// </summary>
        /// <value>
        /// The credential.
        /// </value>
        public string credential { get; set; }
        /// <summary>
        /// Gets or sets the type of the credential.
        /// </summary>
        /// <value>
        /// The type of the credential.
        /// </value>
        public string credentialType { get; set; }
        /// <summary>
        /// Gets or sets the verification level.
        /// </summary>
        /// <value>
        /// The verification level.
        /// </value>
        public string verificationLevel { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UserCredential
    {
        /// <summary>
        /// Gets or sets the credential.
        /// </summary>
        /// <value>
        /// The credential.
        /// </value>
        public string credential { get; set; }
        /// <summary>
        /// Gets or sets the type of the credential.
        /// </summary>
        /// <value>
        /// The type of the credential.
        /// </value>
        public string credentialType { get; set; }
        /// <summary>
        /// Gets or sets the verification level.
        /// </summary>
        /// <value>
        /// The verification level.
        /// </value>
        public string verificationLevel { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Name
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string firstName { get; set; }
        /// <summary>
        /// Gets or sets the name of the middle.
        /// </summary>
        /// <value>
        /// The name of the middle.
        /// </value>
        public string middleName { get; set; }
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string lastName { get; set; }
        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>
        /// The name of the company.
        /// </value>
        public object companyName { get; set; }
        /// <summary>
        /// Gets or sets the name of the department.
        /// </summary>
        /// <value>
        /// The name of the department.
        /// </value>
        public object departmentName { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PhoneNumber
    {
        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        public string phoneNumber { get; set; }
        /// <summary>
        /// Gets or sets the type of the phone number.
        /// </summary>
        /// <value>
        /// The type of the phone number.
        /// </value>
        public string phoneNumberType { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MediaconnectProfile
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int userId { get; set; }
        /// <summary>
        /// Gets or sets the credential.
        /// </summary>
        /// <value>
        /// The credential.
        /// </value>
        public Credential credential { get; set; }
        /// <summary>
        /// Gets or sets the login credential.
        /// </summary>
        /// <value>
        /// The login credential.
        /// </value>
        public LoginCredential loginCredential { get; set; }
        /// <summary>
        /// Gets or sets the user credentials.
        /// </summary>
        /// <value>
        /// The user credentials.
        /// </value>
        public List<UserCredential> userCredentials { get; set; }
        /// <summary>
        /// Gets or sets the customer number.
        /// </summary>
        /// <value>
        /// The customer number.
        /// </value>
        public int customerNumber { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public Name name { get; set; }
        /// <summary>
        /// Gets or sets the phone numbers.
        /// </summary>
        /// <value>
        /// The phone numbers.
        /// </value>
        public List<PhoneNumber> phoneNumbers { get; set; }
        /// <summary>
        /// Gets or sets the emails.
        /// </summary>
        /// <value>
        /// The emails.
        /// </value>
        public List<string> emails { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        public string uniqueId { get; set; }
    }
}