using MailKit.Net.Smtp;
using MimeKit;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Azure.Communication.Email;
using Azure;
using System;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System.IO;

namespace FunctionApp1
{
    public static class EmailSender
    {
        public static async Task<List<string>> AzureEmail(Payload request)
        {
            string senderAddress = "donotreply@8f5b6447-5771-44f3-9b18-a0d44ae3b690.azurecomm.net";
            var AzconnectionString = "endpoint=https://aln-dev-communication-service-resource.communication.azure.com/;accesskey=kN/ncxpNWyPefTuAPwJQWDob//eEt0peBuTAVxS3+dT2h1tJaaM6/QvdKtFhpE0sH9ktmGNgEegV6GDUaWswRQ=="; // Find your Communication Services resource in the Azure portal
            EmailClient emailClient = new EmailClient(AzconnectionString);
            string subject = "Test email with azure communication service" ;

            //string body = htmlContent;
            string htmlContent = File.ReadAllText("C:\\Users\\sumit.gore\\source\\repos\\FunctionApp1\\FunctionApp1\\customtemplate\\index.html");

            string startBlock = "<block1>";
            string endBlock = "</block1>";
            int startIndex = htmlContent.IndexOf(startBlock);
            int endIndex = htmlContent.IndexOf(endBlock, startIndex + startBlock.Length);
            string result = htmlContent.Remove(startIndex, endIndex + endBlock.Length - startIndex);

            List<string>finalResponse = new List<string>();

                
            foreach (var recipient in request.Recipients) {
            string body = IsSpecialRecipient(recipient.Email) ? htmlContent : result;
            string emp_email= recipient.Email;

                try
                {
                    var emailSendOperation = await emailClient.SendAsync(
                        wait: WaitUntil.Completed,
                        senderAddress: senderAddress, // The email address of the domain registered with the Communication Services resource
                        recipientAddress: emp_email,
                        subject: subject,
                        htmlContent: body);

                    Console.WriteLine($"Email Sent. Status = {emailSendOperation.Value.Status}");

                    // Get the OperationId so that it can be used for tracking the message for troubleshooting
                    string operationId = emailSendOperation.Id;
                    Console.WriteLine($"Email operation id = {operationId}");
                    string emailResponse = $"Email Status : {emailSendOperation.Value.Status}   OperationId : {operationId}  Recipient : {recipient}";
                    finalResponse.Add(emailResponse);
                    //return (emailResponse);

                }
                catch (RequestFailedException ex)
                {
                    // OperationID is contained in the exception message and can be used for troubleshooting purposes
                    Console.WriteLine($"Email send operation failed with error code: {ex.ErrorCode}, message: {ex.Message}");
                    finalResponse.Add(ex.Message);
                    //return ex.Message;
                }
            }
                
            return finalResponse;
            
            

        }
        private static bool IsSpecialRecipient(string email)
        {
            // Implement your conditions for special recipients
            // For example, check if the email address belongs to a certain person or position
            if (email == "sumeet.goliwar@sbits.co" || email.EndsWith("@specialposition.com"))
            {
                return true;
            }

            return false;
        }
    }
}
