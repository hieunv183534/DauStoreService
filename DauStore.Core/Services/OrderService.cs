using DauStore.Core.Entities;
using DauStore.Core.Enums;
using DauStore.Core.Interfaces.IRepositories;
using DauStore.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Services
{
    public class OrderService : BaseService<Order>, IOrderService
    {

        protected IOrderRepository _orderRepository;
        protected IItemRepository _itemRepository;
        protected IBaseRepository<Voucher> _voucherRepository;

        public OrderService(IBaseRepository<Order> baseRepository, IOrderRepository orderRepository, IItemRepository itemRepository, IBaseRepository<Voucher> voucherRepository) : base(baseRepository)
        {
            _orderRepository = orderRepository;
            _itemRepository = itemRepository;
            _voucherRepository = voucherRepository;
        }

        public ServiceResult GetOrders(OrderStatusEnum orderStatus, string searchTerms, double startTime, double endTime, int orderTimeState, int index, int count)
        {
            try
            {

                var result = _orderRepository.GetOrders(orderStatus, searchTerms, startTime, endTime, orderTimeState, index, count);
                List<Order> data = (List<Order>)result.GetType().GetProperty("data").GetValue(result, null);
                if (data.Count > 0)
                {
                    _serviceResult.Response = new ResponseModel(2000, "Ok", result);
                    _serviceResult.StatusCode = 200;
                    return _serviceResult;
                }
                else
                {
                    _serviceResult.Response = new ResponseModel(2004, "Không có bản ghi nào!", result);
                    _serviceResult.StatusCode = 204;
                    return _serviceResult;
                }
            }
            catch (Exception ex)
            {
                _serviceResult.Response = new ResponseModel(9999, "Exception Error", new { msg = ex.Message });
                _serviceResult.StatusCode = 500;
                return _serviceResult;
            }
        }

        public override ServiceResult Add(Order order)
        {
            
            try
            {
                // xử lí nghiệp vụ trước khi add order
                var newCode = _orderRepository.GetNewCode(); 
                order.OrderCode = newCode;
                order.PaymentStatus = false;
                order.Status = OrderStatusEnum.Pending;
                order.PaymentMethod = PaymentMethodEnum.COD;
                object calculateRS =  CalculateMoney(order.Items);
                order.Items = (string)calculateRS.GetType().GetProperty("items").GetValue(calculateRS, null);
                order.TotalMoney = (double)calculateRS.GetType().GetProperty("totalMoney").GetValue(calculateRS, null);


                // áp dụng voucher
                if(!Guid.Equals(order.VoucherId, Guid.Empty))
                {
                    Voucher voucher = _voucherRepository.GetById(order.VoucherId);
                    if(voucher == null)
                    {
                        _serviceResult.Response = new ResponseModel(4001, "Voucher đi kèm không đúng!");
                        _serviceResult.StatusCode = 400;
                        return _serviceResult;
                    }
                    else if(voucher.Quota == 0 || (voucher.DateExpired < DateTime.Now))
                    {
                        _serviceResult.Response = new ResponseModel(4001, "Voucher đã hết hạn hoặc hết số lượng!");
                        _serviceResult.StatusCode = 400;
                        return _serviceResult;
                    }

                    if(voucher.SaleNumber != 0)
                    {
                        order.TotalMoney = order.TotalMoney - voucher.SaleNumber;
                    }
                    else
                    {
                        if(order.TotalMoney >= voucher.MinTotal)
                        {
                            double sale = order.TotalMoney * voucher.SaleRate;
                            sale = (sale > voucher.MaxNumber) ? voucher.MaxNumber : sale;
                            order.TotalMoney = (int)(order.TotalMoney - sale);
                        }
                        else
                        {
                            _serviceResult.Response = new ResponseModel(4001, "Số tiền đơn hàng chưa đủ để nhận voucher!");
                            _serviceResult.StatusCode = 400;
                            return _serviceResult;
                        }
                    }
                }
                    

                order.Ship = "Đơn hàng trên 200k sẽ được free ship! Hàng sẽ được giao từ 2-7 ngày tùy nơi đặt.";
                order.ShipPayStatus = ( order.TotalMoney > 200000 ) ? true : false; // đơn hàng trên 200k sẽ được free ship

                // validate
                var validateRs = Validate(order, "add");

                if (validateRs.Code != -1)
                {
                    _serviceResult.Response = validateRs;
                    _serviceResult.StatusCode = 400;
                    return _serviceResult;
                }

                // thêm dữ liệu vào db
                var rowAffect = _baseRepository.Add(order);
                if (rowAffect > 0)
                {
                    _serviceResult.Response = new ResponseModel(2001, "Cập nhật, thêm dữ liệu thành công", new { orderCode = newCode });
                    _serviceResult.StatusCode = 201;
                    return _serviceResult;
                }
                else
                {
                    _serviceResult.Response = new ResponseModel(5005, "Lỗi không xác định");
                    _serviceResult.StatusCode = 500;
                    return _serviceResult;
                }
            }
            catch (Exception ex)
            {
                _serviceResult.Response = new ResponseModel(9999, "Exception Error", new { msg = ex.Message });
                _serviceResult.StatusCode = 500;
                return _serviceResult;
            }
        }
        
        public ServiceResult UpdateOrderByAdmin(Order newOrder, Guid orderId)
        {
            try
            {
                // xử lí nghiệp vụ trước khi update Order by admin
                Order order = _orderRepository.GetById(orderId);
                if(order == null)
                {
                    _serviceResult.Response = new ResponseModel(4001, "OrderId không đúng");
                    _serviceResult.StatusCode = 400;
                    return _serviceResult;
                }
                order.Status = (newOrder.Status != 0) ? newOrder.Status : order.Status;
                order.PaymentStatus = newOrder.PaymentStatus;
                order.ShipPayStatus = newOrder.ShipPayStatus;
                //order.Ship = newOrder.Ship;

                // validate
                var validateRs = Validate(order, "update");
                if (validateRs.Code != -1)
                {
                    _serviceResult.Response = validateRs;
                    _serviceResult.StatusCode = 400;
                    return _serviceResult;
                }

                // thêm dữ liệu vào db
                var rowAffect = _baseRepository.Update(order, orderId);
                if (rowAffect > 0)
                {
                    _serviceResult.Response = new ResponseModel(2001, "OK", rowAffect);
                    _serviceResult.StatusCode = 201;
                    return _serviceResult;
                }
                else
                {
                    _serviceResult.Response = new ResponseModel(5005, "Unknown Error");
                    _serviceResult.StatusCode = 500;
                    return _serviceResult;
                }
            }
            catch (Exception ex)
            {
                _serviceResult.Response = new ResponseModel(9999, "Exception Error", new { msg = ex.Message });
                _serviceResult.StatusCode = 500;
                return _serviceResult;
            }
        }

        public ServiceResult UpdateOrderByCustomer(Order newOrder, Guid orderId)
        {
            try
            {
                // xử lí nghiệp vụ trước khi update Order by admin
                Order order = _orderRepository.GetById(orderId);
                if (order == null)
                {
                    _serviceResult.Response = new ResponseModel(4001, "OrderId không đúng");
                    _serviceResult.StatusCode = 400;
                    return _serviceResult;
                }
                if(order.Status != OrderStatusEnum.Pending)
                {
                    _serviceResult.Response = new ResponseModel(4001, "Đơn hàng đã được duyệt hoặc đang giao, Vui lòng liên hệ admin để có thể cập nhật đơn hàng!");
                    _serviceResult.StatusCode = 400;
                    return _serviceResult;
                }

                order.BuyerName = (newOrder.BuyerName !=  null) ? newOrder.BuyerName : order.BuyerName;
                order.UnitCode = (newOrder.UnitCode != null) ? newOrder.UnitCode : order.UnitCode ;
                order.Phone = (newOrder.Phone != null) ? newOrder.Phone : order.Phone;
                order.Address = (newOrder.Address != null) ? newOrder.Address : order.Address;
                order.PaymentMethod = (newOrder.PaymentMethod != 0) ? newOrder.PaymentMethod : order.PaymentMethod;
                if (newOrder.Items != null)
                {
                    object calculateRS = CalculateMoney(newOrder.Items);
                    order.Items = (string)calculateRS.GetType().GetProperty("items").GetValue(calculateRS, null);
                    order.TotalMoney = (double)calculateRS.GetType().GetProperty("totalMoney").GetValue(calculateRS, null);
                }

                // áp dụng voucher
                if (!Guid.Equals(newOrder.VoucherId, Guid.Empty))
                {
                    order.VoucherId = newOrder.VoucherId;
                    Voucher voucher = _voucherRepository.GetById(order.VoucherId);
                    if (voucher == null)
                    {
                        _serviceResult.Response = new ResponseModel(4001, "Voucher đi kèm không đúng!");
                        _serviceResult.StatusCode = 400;
                        return _serviceResult;
                    }
                    else if (voucher.Quota == 0 || (voucher.DateExpired < DateTime.Now))
                    {
                        _serviceResult.Response = new ResponseModel(4001, "Voucher đã hết hạn hoặc hết số lượng!");
                        _serviceResult.StatusCode = 400;
                        return _serviceResult;
                    }

                    if (voucher.SaleNumber != 0)
                    {
                        order.TotalMoney = order.TotalMoney - voucher.SaleNumber;
                    }
                    else
                    {
                        if (order.TotalMoney >= voucher.MinTotal)
                        {
                            double sale = order.TotalMoney * voucher.SaleRate;
                            sale = (sale > voucher.MaxNumber) ? voucher.MaxNumber : sale;
                            order.TotalMoney = (int)(order.TotalMoney - sale);
                        }
                        else
                        {
                            _serviceResult.Response = new ResponseModel(4001, "Số tiền đơn hàng chưa đủ để nhận voucher!");
                            _serviceResult.StatusCode = 400;
                            return _serviceResult;
                        }
                    }
                }

                order.Ship = "Đơn hàng trên 200k sẽ được free ship! Hàng sẽ được giao từ 2-7 ngày tùy nơi đặt.";
                order.ShipPayStatus = (order.TotalMoney > 200000) ? true : false; // đơn hàng trên 200k sẽ được free ship

                // validate
                var validateRs = Validate(order, "update");
                if (validateRs.Code != -1)
                {
                    _serviceResult.Response = validateRs;
                    _serviceResult.StatusCode = 400;
                    return _serviceResult;
                }

                // thêm dữ liệu vào db
                var rowAffect = _baseRepository.Update(order, orderId);
                if (rowAffect > 0)
                {
                    _serviceResult.Response = new ResponseModel(2001, "OK", rowAffect);
                    _serviceResult.StatusCode = 201;
                    return _serviceResult;
                }
                else
                {
                    _serviceResult.Response = new ResponseModel(5005, "Unknown Error");
                    _serviceResult.StatusCode = 500;
                    return _serviceResult;
                }
            }
            catch (Exception ex)
            {
                _serviceResult.Response = new ResponseModel(9999, "Exception Error", new { msg = ex.Message });
                _serviceResult.StatusCode = 500;
                return _serviceResult;
            }
        }

        private Object CalculateMoney(string itemsText)
        {
            double totalMoney = 0;
            var itemArrStr = itemsText.Trim().Split(' ');
            List<string> items = new List<string>();
            for (int i = 0; i < itemArrStr.Length; i++)
            {
                Guid itemId = Guid.Parse(itemArrStr[i].Split('|')[1]);
                int quantity = Int32.Parse(itemArrStr[i].Split('|')[0]);
                var item = _itemRepository.GetById(itemId);
                if (item == null)
                {
                    throw new Exception("Id item trong itemText không đúng. Không tìm thấy item tương ứng");
                }
                int itemMoney = (int)(quantity*(item.RealPrice *(1 - item.SaleRate)));
                totalMoney += itemMoney;
                items.Add($"{quantity}|{itemId}|{itemMoney}");
            }
            return new
            {
                totalMoney = totalMoney,
                items = String.Join(' ', items)
            };
        }

        public ServiceResult LookupOrder(string key)
        {
            try
            {
                key = (key == null) ? "" : key;
                var orders = _orderRepository.LookupOrder(key);
                if (orders.Count > 0)
                {
                    _serviceResult.Response = new ResponseModel(2000, "Ok", orders);
                    _serviceResult.StatusCode = 200;
                    return _serviceResult;
                }
                else
                {
                    _serviceResult.Response = new ResponseModel(2004, "Không có bản ghi nào!", orders);
                    _serviceResult.StatusCode = 204;
                    return _serviceResult;
                }
            }
            catch (Exception ex)
            {
                _serviceResult.Response = new ResponseModel(9999, "Exception Error", new { msg = ex.Message });
                _serviceResult.StatusCode = 500;
                return _serviceResult;
            }
        }
    }
}
