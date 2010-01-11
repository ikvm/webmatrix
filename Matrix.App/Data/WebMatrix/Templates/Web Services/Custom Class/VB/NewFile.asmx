<%@ WebService language="VB" class="%%ClassName%%" %>

Imports System
Imports System.Web.Services
Imports System.Xml.Serialization
Imports System.Web.Services.Protocols

Public Class %%ClassName%%

    <WebMethod> Public Function GetOrderDetails() 
        Dim orderDetails As New OrderDetails()

        ' Set values for OrderDetails class
        orderDetails.OrderNumber = 35
        orderDetails.CompanyName = "ACME Paint"
        orderDetails.CustomerFirstName = "John"
        orderDetails.CustomerLastName = "Smith"
        orderDetails.CustomerEmail = "John.Smith@IBuySpy.com"

        ' Return an array of 2 OrderItems
        orderDetails.OrderItems As New OrderItem(2);

        orderDetails.OrderItems(0).ItemName = "Sunset Yellow"
        orderDetails.OrderItems(0).ItemId = 12
        orderDetails.OrderItems(0).ItemName = "You'll feel like you're at the beach"

        orderDetails.OrderItems(1).ItemName = "Seattle Sky Blue"
        orderDetails.OrderItems(1).ItemId = 93
        orderDetails.OrderItems(1).ItemName = "A rare shade of blue"

        Return orderDetails
    End Function

End Class

Public Class OrderDetails 

    <XmlAttribute("OrderId")> Public OrderNumber As Integer     ' Serialize as an attribute named OrderId

    <XmlAttribute()> Public CompanyName As String              ' Serialize as an attribute

    Public CustomerFirstName As String

    Public CustomerLastName As String

    <XmlElement("email")> Public CustomerEmail As String        ' Serialize as an element, but change the name

    <XmlArray("OrderItemDetail")> Public OrderItems As OrderItem() ' Rename the array OrderItemDetail
End Class

Public Class OrderItem

    ' Serialize all member variables as attributes

    <XmlAttribute()> Public ItemName As String

    <XmlAttribute()> Public ItemId As Integer

    <XmlAttribute()> Public ItemDescription As String
End Class
