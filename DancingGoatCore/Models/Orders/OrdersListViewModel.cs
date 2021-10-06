using System;

using CMS.Ecommerce;

namespace DancingGoat.Models
{
    public class OrdersListViewModel
    {
        public int OrderID { get; set; }


        public string OrderInvoiceNumber { get; set; }


        public DateTime OrderDate { get; set; }


        public string StatusName { get; set; }


        public string FormattedTotalPrice { get; set; }


        public OrdersListViewModel()
        {
        }


        public OrdersListViewModel(OrderInfo order, ICurrencyInfoProvider currencyInfoProvider, IOrderStatusInfoProvider orderStatusInfoProvider)
        {
            if (order == null)
            {
                return;
            }

            if (currencyInfoProvider == null)
            {
                throw new ArgumentNullException(nameof(currencyInfoProvider));
            }

            if (orderStatusInfoProvider == null)
            {
                throw new ArgumentNullException(nameof(orderStatusInfoProvider));
            }

            OrderID = order.OrderID;
            OrderInvoiceNumber = order.OrderInvoiceNumber;
            OrderDate = order.OrderDate;
            StatusName = orderStatusInfoProvider.Get(order.OrderStatusID)?.StatusDisplayName;
            FormattedTotalPrice = String.Format(currencyInfoProvider.Get(order.OrderCurrencyID).CurrencyFormatString, order.OrderTotalPrice);
        }
    }
}