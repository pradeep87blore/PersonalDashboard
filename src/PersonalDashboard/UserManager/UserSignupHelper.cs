using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.CognitoIdentity;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace UserManager
{
    public static class CognitoHashCalculator
    {
        public static string CreateSecretHash(string userName, string appClientId, string appSecretKey)
        {
            var temp = userName + appClientId;

            var bytes = Encoding.UTF8.GetBytes(temp);
            var secretKey = Encoding.UTF8.GetBytes(appSecretKey);

            return Convert.ToBase64String(HmacSHA256(bytes, secretKey));
        }

        public static byte[] HmacSHA256(byte[] data, byte[] key)
        {
            using (var shaAlgorithm = new System.Security.Cryptography.HMACSHA256(key))
            {
                var result = shaAlgorithm.ComputeHash(data);
                return result;
            }
        }
    }

    public class UserSignupHelper
    {
        public static bool SignupUser(string userName, string password)
        {

            string ClientAppId = SecretFetcher.GetSecretValue(SecretNames.CLIENT_ID);
            string ClientSecret = SecretFetcher.GetSecretValue(SecretNames.CLIENT_SECRET);
            string AWSRegion = SecretFetcher.GetSecretValue(SecretNames.AWS_REGION);
            AmazonCognitoIdentityProviderClient provider =
               new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), Amazon.RegionEndpoint.USEast1); // TODO: How to set the endpoint dynamically?

            SignUpRequest signUpRequest = new SignUpRequest();
            signUpRequest.ClientId = ClientAppId;
            signUpRequest.Password = password;
            signUpRequest.Username = userName;

            AttributeType attributeType1 = new AttributeType();
            attributeType1.Name = "email";
            attributeType1.Value = userName; //email;
            signUpRequest.UserAttributes.Add(attributeType1);

            signUpRequest.SecretHash = CognitoHashCalculator.CreateSecretHash(signUpRequest.Username, ClientAppId, ClientSecret);

            try
            {
                SignUpResponse result = provider.SignUp(signUpRequest);

                if (result.HttpStatusCode != System.Net.HttpStatusCode.OK)
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }

        public static bool ValidateUser(string userName, string code)
        {
            string ClientAppId = SecretFetcher.GetSecretValue(SecretNames.CLIENT_ID);
            string ClientSecret = SecretFetcher.GetSecretValue(SecretNames.CLIENT_SECRET);

            AmazonCognitoIdentityProviderClient provider =
               new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), Amazon.RegionEndpoint.USEast1);
            ConfirmSignUpRequest confirmSignUpRequest = new ConfirmSignUpRequest();
            confirmSignUpRequest.Username = userName;
            confirmSignUpRequest.ConfirmationCode = code;
            confirmSignUpRequest.ClientId = ClientAppId;

            confirmSignUpRequest.SecretHash = CognitoHashCalculator.CreateSecretHash(confirmSignUpRequest.Username, ClientAppId, ClientSecret);

            try
            {
                ConfirmSignUpResponse confirmSignUpResult = provider.ConfirmSignUp(confirmSignUpRequest);
                Console.WriteLine(confirmSignUpResult.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return true;
        }

        //internal static async Task<string> GetS3BucketsAsync(CognitoUser user)
        //{
        //    string userPoolId = SecretFetcher.GetSecretValue(SecretNames.USER_POOL_ID);

        //    CognitoAWSCredentials credentials =
        //       user.GetCognitoAWSCredentials(userPoolId, Amazon.RegionEndpoint.USEast1);
        //    StringBuilder bucketlist = new StringBuilder();

        //    bucketlist.Append("================Cognito Credentails==================\n");
        //    bucketlist.Append("Access Key - " + credentials.GetCredentials().AccessKey);
        //    bucketlist.Append("\nSecret - " + credentials.GetCredentials().SecretKey);
        //    bucketlist.Append("\nSession Token - " + credentials.GetCredentials().Token);

        //    bucketlist.Append("\n================User Buckets==================\n");

        //    using (var client = new AmazonS3Client(credentials))
        //    {
        //        ListBucketsResponse response =
        //            await client.ListBucketsAsync(new ListBucketsRequest()).ConfigureAwait(false);

        //        foreach (var bucket in response.Buckets)
        //        {
        //            bucketlist.Append(bucket.BucketName);

        //            bucketlist.Append("\n");
        //        }
        //    }
        //    Console.WriteLine(bucketlist.ToString());
        //    return bucketlist.ToString();
        //}


        //internal static async Task<CognitoUser> GetCognitoUser(string username, string password)
        //{
        //    string ClientAppId = SecretFetcher.GetSecretValue(SecretNames.CLIENT_ID);
        //    string UserPoolId = SecretFetcher.GetSecretValue(SecretNames.USER_POOL_ID);

        //    AmazonCognitoIdentityProviderClient provider =
        //            new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), Amazon.RegionEndpoint.USEast1);

        //    CognitoUserPool userPool = new CognitoUserPool(UserPoolId, ClientAppId, provider);

        //    CognitoUser user = new CognitoUser(username, ClientAppId, userPool, provider);
        //    InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest()
        //    {
        //        Password = password
        //    };


        //    AuthFlowResponse authResponse = await user.StartWithSrpAuthAsync(authRequest).ConfigureAwait(false);
        //    if (authResponse.AuthenticationResult != null)
        //    {
        //        return user;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //} 
        
        //public static async Task<string> ListUserBuckets(string username, string password)
        //{
        //    var cUser = GetCognitoUser(username, password);

        //    return await GetS3BucketsAsync(cUser.Result);
        //}
    }
}
