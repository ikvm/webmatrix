<%@ WebService language="C#" class="%%ClassName%%" %>

using System;
using System.Web.Services;
using System.Xml.Serialization;
using System.Web.Services.Protocols;

public class %%ClassName%% {

    [WebMethod]
    public OrderDetails GetOrderDetails() {
        OrderDetails orderDetails = new OrderDetails();

        // Set values for OrderDetails class
        orderDetails.OrderNumber = 35;
        orderDetails.CompanyName = "ACME Paint";
        orderDetails.CustomerFirstName = "John";
        orderDetails.CustomerLastName = "Smith";
        orderDetails.CustomerEmail = "John.Smith@IBuySpy.com";

        // Return an array of 2 OrderItems
        orderDetails.OrderItems = new OrderItem[2];

        orderDetails.OrderItems[0] = new OrderItem();
        orderDetails.OrderItems[0].ItemName = "Sunset Yellow";
        orderDetails.OrderItems[0].ItemId = 12;
        orderDetails.OrderItems[0].ItemName = "You'll feel like you're at the beach";

        orderDetails.OrderItems[1] = new OrderItem();
        orderDetails.OrderItems[1].ItemName = "Seattle Sky Blue";
        orderDetails.OrderItems[1].ItemId = 93;
        orderDetails.OrderItems[1].ItemName = "A rare shade of blue";

        return orderDetails;
            
    }
}

public class OrderDetails {

    [XmlAttribute("OrderId")]
    public int OrderNumber;           // Serialize as an attribute named OrderId

    [XmlAttribute()]
    public string CompanyName;        // Serialize as an attribute

    public string CustomerFirstName;

    public string CustomerLastName;

    [XmlElement("email")]
    public string CustomerEmail;      // Serialize as an element, but change the name

    [XmlArray("OrderItemDetail")]
    public OrderItem[] OrderItems;    // Rename the array OrderItemDetail
}

public class OrderItem {

    // Serialize all member variables as attributes

    [XmlAttribute()]
    public string ItemName;

    [XmlAttribute()]
    public int ItemId;

    [XmlAttribute()]
    public string ItemDescription;
}
