namespace Microsoft.Matrix.Core.UserInterface
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Web.Services;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.Web.Services.Description;

    [WebServiceBinding(Name="ComponentGalleryServiceSoap", Namespace="http://www.asp.net/matrix/services/ComponentGalleryService"), DebuggerStepThrough, DesignerCategory("code")]
    public class ComponentGalleryServiceProxy : SoapHttpClientProtocol
    {
        public ComponentGalleryServiceProxy(string url)
        {
            base.Url = (url);
        }

        public IAsyncResult BeginFindComponents(string keywords, string typeFilter, AsyncCallback callback, object asyncState)
        {
            return base.BeginInvoke("FindComponents", new object[] { keywords, typeFilter }, callback, asyncState);
        }

        public IAsyncResult BeginGetCategories(string typeFilter, AsyncCallback callback, object asyncState)
        {
            return base.BeginInvoke("GetCategories", new object[] { typeFilter }, callback, asyncState);
        }

        public IAsyncResult BeginGetComponent(int componentId, AsyncCallback callback, object asyncState)
        {
            return base.BeginInvoke("GetComponent", new object[] { componentId }, callback, asyncState);
        }

        public IAsyncResult BeginGetComponents(int categoryId, string typeFilter, AsyncCallback callback, object asyncState)
        {
            return base.BeginInvoke("GetComponents", new object[] { categoryId, typeFilter }, callback, asyncState);
        }

        public IAsyncResult BeginGetComponentTypes(AsyncCallback callback, object asyncState)
        {
            return base.BeginInvoke("GetComponentTypes", new object[0], callback, asyncState);
        }

        public IAsyncResult BeginGetPreview(int componentId, AsyncCallback callback, object asyncState)
        {
            return base.BeginInvoke("GetPreview", new object[] { componentId }, callback, asyncState);
        }

        public IAsyncResult BeginGetReviews(int componentId, int startIndex, int count, AsyncCallback callback, object asyncState)
        {
            return base.BeginInvoke("GetReviews", new object[] { componentId, startIndex, count }, callback, asyncState);
        }

        public IAsyncResult BeginSubmitComponent(string packageUrl, AsyncCallback callback, object asyncState)
        {
            return base.BeginInvoke("SubmitComponent", new object[] { packageUrl }, callback, asyncState);
        }

        public IAsyncResult BeginSubmitReview(int componentId, string title, string email, string contents, int rating, AsyncCallback callback, object asyncState)
        {
            return base.BeginInvoke("SubmitReview", new object[] { componentId, title, email, contents, rating }, callback, asyncState);
        }

        public ComponentInfo[] EndFindComponents(IAsyncResult asyncResult)
        {
            return (ComponentInfo[]) base.EndInvoke(asyncResult)[0];
        }

        public CategoryInfo[] EndGetCategories(IAsyncResult asyncResult)
        {
            return (CategoryInfo[]) base.EndInvoke(asyncResult)[0];
        }

        public ComponentDescription EndGetComponent(IAsyncResult asyncResult)
        {
            return (ComponentDescription) base.EndInvoke(asyncResult)[0];
        }

        public ComponentInfo[] EndGetComponents(IAsyncResult asyncResult)
        {
            return (ComponentInfo[]) base.EndInvoke(asyncResult)[0];
        }

        public ComponentTypeInfo[] EndGetComponentTypes(IAsyncResult asyncResult)
        {
            return (ComponentTypeInfo[]) base.EndInvoke(asyncResult)[0];
        }

        public byte[] EndGetPreview(IAsyncResult asyncResult)
        {
            return (byte[]) base.EndInvoke(asyncResult)[0];
        }

        public Review[] EndGetReviews(IAsyncResult asyncResult)
        {
            return (Review[]) base.EndInvoke(asyncResult)[0];
        }

        public bool EndSubmitComponent(IAsyncResult asyncResult)
        {
            return (bool) base.EndInvoke(asyncResult)[0];
        }

        public void EndSubmitReview(IAsyncResult asyncResult)
        {
            base.EndInvoke(asyncResult);
        }

        [SoapDocumentMethod("http://www.asp.net/matrix/services/ComponentGalleryService/FindComponents", RequestNamespace = "http://www.asp.net/matrix/services/ComponentGalleryService", ResponseNamespace = "http://www.asp.net/matrix/services/ComponentGalleryService", Use = (SoapBindingUse)2, ParameterStyle = (SoapParameterStyle)2)]
        public ComponentInfo[] FindComponents(string keywords, string typeFilter)
        {
            return (ComponentInfo[]) base.Invoke("FindComponents", new object[] { keywords, typeFilter })[0];
        }

        [SoapDocumentMethod("http://www.asp.net/matrix/services/ComponentGalleryService/GetCategories", RequestNamespace = "http://www.asp.net/matrix/services/ComponentGalleryService", ResponseNamespace = "http://www.asp.net/matrix/services/ComponentGalleryService", Use = (SoapBindingUse)2, ParameterStyle = (SoapParameterStyle)2)]
        public CategoryInfo[] GetCategories(string typeFilter)
        {
            return (CategoryInfo[]) base.Invoke("GetCategories", new object[] { typeFilter })[0];
        }

        [SoapDocumentMethod("http://www.asp.net/matrix/services/ComponentGalleryService/GetComponent", RequestNamespace = "http://www.asp.net/matrix/services/ComponentGalleryService", ResponseNamespace = "http://www.asp.net/matrix/services/ComponentGalleryService", Use = (SoapBindingUse)2, ParameterStyle = (SoapParameterStyle)2)]
        public ComponentDescription GetComponent(int componentId)
        {
            return (ComponentDescription) base.Invoke("GetComponent", new object[] { componentId })[0];
        }

        [SoapDocumentMethod("http://www.asp.net/matrix/services/ComponentGalleryService/GetComponents", RequestNamespace = "http://www.asp.net/matrix/services/ComponentGalleryService", ResponseNamespace = "http://www.asp.net/matrix/services/ComponentGalleryService", Use = (SoapBindingUse)2, ParameterStyle = (SoapParameterStyle)2)]
        public ComponentInfo[] GetComponents(int categoryId, string typeFilter)
        {
            return (ComponentInfo[]) base.Invoke("GetComponents", new object[] { categoryId, typeFilter })[0];
        }

        [SoapDocumentMethod("http://www.asp.net/matrix/services/ComponentGalleryService/GetComponentTypes", RequestNamespace = "http://www.asp.net/matrix/services/ComponentGalleryService", ResponseNamespace = "http://www.asp.net/matrix/services/ComponentGalleryService", Use = (SoapBindingUse)2, ParameterStyle = (SoapParameterStyle)2)]
        public ComponentTypeInfo[] GetComponentTypes()
        {
            return (ComponentTypeInfo[]) base.Invoke("GetComponentTypes", new object[0])[0];
        }

        [return: XmlElement(DataType="base64Binary")]
        [SoapDocumentMethod("http://www.asp.net/matrix/services/ComponentGalleryService/GetPreview", RequestNamespace = "http://www.asp.net/matrix/services/ComponentGalleryService", ResponseNamespace = "http://www.asp.net/matrix/services/ComponentGalleryService", Use = (SoapBindingUse)2, ParameterStyle = (SoapParameterStyle)2)]
        public byte[] GetPreview(int componentId)
        {
            return (byte[]) base.Invoke("GetPreview", new object[] { componentId })[0];
        }

        [SoapDocumentMethod("http://www.asp.net/matrix/services/ComponentGalleryService/GetReviews", RequestNamespace = "http://www.asp.net/matrix/services/ComponentGalleryService", ResponseNamespace = "http://www.asp.net/matrix/services/ComponentGalleryService", Use = (SoapBindingUse)2, ParameterStyle = (SoapParameterStyle)2)]
        public Review[] GetReviews(int componentId, int startIndex, int count)
        {
            return (Review[]) base.Invoke("GetReviews", new object[] { componentId, startIndex, count })[0];
        }

        [SoapDocumentMethod("http://www.asp.net/matrix/services/ComponentGalleryService/SubmitComponent", RequestNamespace = "http://www.asp.net/matrix/services/ComponentGalleryService", ResponseNamespace = "http://www.asp.net/matrix/services/ComponentGalleryService", Use = (SoapBindingUse)2, ParameterStyle = (SoapParameterStyle)2)]
        public bool SubmitComponent(string packageUrl)
        {
            return (bool) base.Invoke("SubmitComponent", new object[] { packageUrl })[0];
        }

        [SoapDocumentMethod("http://www.asp.net/matrix/services/ComponentGalleryService/SubmitReview", RequestNamespace = "http://www.asp.net/matrix/services/ComponentGalleryService", ResponseNamespace = "http://www.asp.net/matrix/services/ComponentGalleryService", Use = (SoapBindingUse)2, ParameterStyle = (SoapParameterStyle)2)]
        public void SubmitReview(int componentId, string title, string email, string contents, int rating)
        {
            base.Invoke("SubmitReview", new object[] { componentId, title, email, contents, rating });
        }
    }
}

