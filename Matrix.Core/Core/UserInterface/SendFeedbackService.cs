namespace Microsoft.Matrix.Core.UserInterface
{
    using System;
    using System.Web.Services;
    using System.Web.Services.Protocols;
    using System.Web.Services.Description;

    [WebServiceBinding(Name="SendFeedbackServiceSoap", Namespace="http://www.asp.net/matrix/services/SendFeedbackService")]
    public class SendFeedbackService : SoapHttpClientProtocol
    {
        public SendFeedbackService(string url)
        {
            base.Url = (url);
        }

        public IAsyncResult BeginSubmitFeedback(string appId, string userEmail, FeedbackData feedbackData, AsyncCallback callback, object asyncState)
        {
            return base.BeginInvoke("SubmitFeedback", new object[] { appId, userEmail, feedbackData }, callback, asyncState);
        }

        public bool EndSubmitFeedback(IAsyncResult asyncResult)
        {
            return (bool) base.EndInvoke(asyncResult)[0];
        }

        [SoapDocumentMethod("http://www.asp.net/matrix/services/SendFeedbackService/SubmitFeedback", RequestNamespace="http://www.asp.net/matrix/services/SendFeedbackService", ResponseNamespace="http://www.microsoft.net/matrix/services/SendFeedbackService", Use=(SoapBindingUse)2, ParameterStyle = (SoapParameterStyle)2)]
        public bool SubmitFeedback(string appId, string userEmail, FeedbackData feedbackData)
        {
            return (bool) base.Invoke("SubmitFeedback", new object[] { appId, userEmail, feedbackData })[0];
        }
    }
}

