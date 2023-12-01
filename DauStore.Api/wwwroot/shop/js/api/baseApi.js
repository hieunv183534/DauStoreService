class BaseApi {
    constructor() {
        this.baseUrl = 'http://localhost:5000/api/';
        this.accessToken = sessionStorage.getItem('access_token');
    }

    signup(account) {
        return $.ajax({
            url: this.baseUrl + 'signup',
            method: 'POST',
            data: JSON.stringify(account),
            dataType: 'json',
            contentType: 'application/json'
        })
    }

    login(phone, password) {
        return $.ajax({
            url: this.baseUrl + 'login',
            method: 'POST',
            data: JSON.stringify({ phone, password }),
            dataType: 'json',
            contentType: 'application/json'
        })
    }

    logout() {
        return $.ajax({
            url: this.baseUrl + 'logout',
            method: 'POST',
            headers: { "Authorization": this.accessToken },
            dataType: 'json',
            contentType: 'application/json'
        })
    }

    changePassword(password, newPassword) {
        return $.ajax({
            url: this.baseUrl + `changePassword?password=${password}&newPassword=${newPassword}`,
            method: 'POST',
            headers: { "Authorization": this.accessToken },
            dataType: 'json',
            contentType: 'application/json'
        })
    }

    updateAccount(newAccount) {
        return $.ajax({
            url: this.baseUrl + `updateAccount`,
            method: 'POST',
            headers: { "Authorization": this.accessToken },
            data: JSON.stringify(newAccount),
            dataType: 'json',
            contentType: 'application/json'
        })
    }

    getCategorys() {
        return $.ajax({
            url: this.baseUrl + `getCategorys`,
            method: 'GET'
        })
    }

    addCategory(category) {
        console.log(category);
        return $.ajax({
            url: this.baseUrl + `addCategory`,
            method: 'POST',
            headers: { "Authorization": this.accessToken },
            data: JSON.stringify(category),
            dataType: 'json',
            contentType: 'application/json'
        })
    }

    updateCategory(categoryId, newName, listDescription) {
        return $.ajax({
            url: this.baseUrl + `updateCategory/${categoryId}`,
            method: 'PUT',
            headers: { "Authorization": this.accessToken },
            data: JSON.stringify({ categoryName: newName, categoryListDescription: listDescription }),
            dataType: 'json',
            contentType: 'application/json'
        })
    }

    deleteCategory(categoryId) {
        return $.ajax({
            url: this.baseUrl + `deleteCategory/${categoryId}`,
            method: 'DELETE',
            headers: { "Authorization": this.accessToken }
        })
    }

    getItems(categoryCode = null, searchTerms = null, tag = null, orderState = 1, index = 0, count = 50) {
        let _categoryCode = (categoryCode == null) ? '' : `&categoryCode=${categoryCode}`;
        let _searchTerms = (searchTerms == null) ? '' : `&searchTerms=${searchTerms}`;
        let _tag = (tag == null) ? '' : `&tag=${tag}`;
        return $.ajax({
            url: this.baseUrl + `getItems?index=${index}&count=${count}&orderState=${orderState}${_categoryCode}${_searchTerms}${_tag}`,
            method: 'GET'
        })
    }

    getItemById(itemId) {
        return $.ajax({
            url: this.baseUrl + `getItemById/${itemId}`,
            method: 'GET'
        })
    }

    addItem(item) {
        return $.ajax({
            url: this.baseUrl + `addItem`,
            method: 'POST',
            headers: { "Authorization": this.accessToken },
            data: JSON.stringify(item),
            dataType: 'json',
            contentType: 'application/json'
        })
    }

    updateItem(item, itemId) {
        return $.ajax({
            url: this.baseUrl + `updateItem/${itemId}`,
            method: 'PUT',
            headers: { "Authorization": this.accessToken },
            data: JSON.stringify(item),
            dataType: 'json',
            contentType: 'application/json'
        })
    }

    changeItemInStock(itemId, changeNumber) {
        return $.ajax({
            url: this.baseUrl + `changeInStock/${itemId}/${changeNumber}`,
            method: 'PUT',
            headers: { "Authorization": this.accessToken },
        })
    }

    deleteItem(itemId) {
        return $.ajax({
            url: this.baseUrl + `deleteItem/${itemId}`,
            method: 'DELETE',
            headers: { "Authorization": this.accessToken }
        })
    }

    getNewItemCode() {
        return $.ajax({
            url: this.baseUrl + `getNewItemCode`,
            method: 'GET',
            headers: { "Authorization": this.accessToken }
        })
    }

    getNotifications(index = 0, count = 50) {
        return $.ajax({
            url: this.baseUrl + `getNotifications?index=${index}&count=${count}`,
            method: 'GET',
            headers: { "Authorization": this.accessToken }
        })
    }

    addNotification(notification) {
        return $.ajax({
            url: this.baseUrl + `addNotification`,
            method: 'POST',
            headers: { "Authorization": this.accessToken },
            data: JSON.stringify(notification),
            dataType: 'json',
            contentType: 'application/json'
        })
    }

    seenAllNotification() {
        return $.ajax({
            url: this.baseUrl + `seenAllNotification`,
            method: 'POST',
            headers: { "Authorization": this.accessToken },
        })
    }

    getOrderById(orderId) {
        return $.ajax({
            url: this.baseUrl + `getOrderById/${orderId}`,
            method: 'GET',
            headers: { "Authorization": this.accessToken },
        })
    }


    lookupOrder(key) {
        return $.ajax({
            url: this.baseUrl + `lookupOrder/${key}`,
            method: 'GET'
        })
    }

    getOrders(orderStatus = null, searchTerms = null, startTime = null, endTime = null, orderTimeState = 1, index = 0, count = 50) {
        startTime = !startTime ? null : Number(new Date(startTime)) / 1000;
        endTime = !endTime ? null : Number(new Date(endTime)) / 1000;
        let _orderStatus = (!orderStatus) ? '' : `&orderStatus=${orderStatus}`;
        let _searchTerms = (!searchTerms) ? '' : `&searchTerms=${searchTerms}`;
        let _startTime = (!startTime) ? '' : `&startTime=${startTime}`;
        let _endTime = (!endTime) ? '' : `&endTime=${endTime}`;
        console.log(_orderStatus, _searchTerms, _endTime, _startTime);
        return $.ajax({
            url: this.baseUrl + `getOrders?orderTimeState=${orderTimeState}&index=${index}&count=${count}${_orderStatus}${_searchTerms}${_startTime}${_endTime}`,
            method: 'GET',
            headers: { "Authorization": this.accessToken },
        })
    }

    addOrder(order) {
        return $.ajax({
            url: this.baseUrl + `addOrder`,
            method: 'POST',
            data: JSON.stringify(order),
            dataType: 'json',
            contentType: 'application/json'
        })
    }

    updateOrderByAdmin(order, orderId) {
        return $.ajax({
            url: this.baseUrl + `updateOrderByAdmin/${orderId}`,
            method: 'PUT',
            headers: { "Authorization": this.accessToken },
            data: JSON.stringify(order),
            dataType: 'json',
            contentType: 'application/json'
        })
    }

    updateOrderByCustomer(order, orderId) {
        return $.ajax({
            url: this.baseUrl + `updateOrderByCustomer/${orderId}`,
            method: 'PUT',
            data: JSON.stringify(order),
            dataType: 'json',
            contentType: 'application/json'
        })
    }

    deleteOrder(orderId) {
        return $.ajax({
            url: this.baseUrl + `deleteOrder/${orderId}`,
            method: 'DELETE',
            headers: { "Authorization": this.accessToken }
        })
    }

    exportBillDoc(orderId, unitAddress) {
        return $.ajax({
            url: this.baseUrl + `getOrderDocFile/${orderId}?unitAddress=${unitAddress}`,
            method: 'GET',
            headers: { "Authorization": this.accessToken }
        })
    }

    getVouchers(index = 0, count = 50, searchTerms = null, canuseState = 0) {
        let _searchTerms = (searchTerms == null) ? '' : `&searchTerms=${searchTerms}`;
        return $.ajax({
            url: this.baseUrl + `getVouchers?index=${index}&count=${count}&canuseState=${canuseState}${_searchTerms}`,
            method: 'GET',
            headers: { "Authorization": this.accessToken }
        })
    }

    addVoucher(voucher) {
        return $.ajax({
            url: this.baseUrl + `addVoucher`,
            method: 'POST',
            headers: { "Authorization": this.accessToken },
            data: JSON.stringify(voucher),
            dataType: 'json',
            contentType: 'application/json'
        })
    }

    updateVoucher(voucher, voucherId) {
        return $.ajax({
            url: this.baseUrl + `updateVoucher/${voucherId}`,
            method: 'PUT',
            headers: { "Authorization": this.accessToken },
            data: JSON.stringify(voucher),
            dataType: 'json',
            contentType: 'application/json'
        })
    }

    deleteVoucher(voucherId) {
        return $.ajax({
            url: this.baseUrl + `deleteVoucher/${voucherId}`,
            method: 'DELETE',
            headers: { "Authorization": this.accessToken },
        })
    }

    getVoucherByCode(voucherCode) {
        return $.ajax({
            url: this.baseUrl + `getVoucherByCode/${voucherCode}`,
            method: 'GET'
        })
    }

    getVoucherById(voucherId) {
        return $.ajax({
            url: this.baseUrl + `getVoucherById/${voucherId}`,
            method: 'GET'
        })
    }

    getUnit(unitCode) {
        return $.ajax({
            url: `https://n8-covid-be.herokuapp.com/unit/unit-single?unit=${unitCode}`,
            method: 'GET'
        });
    }

    getAccounts() {
        return $.ajax({
            url: this.baseUrl + `getAccounts`,
            method: 'GET',
            headers: { "Authorization": this.accessToken }
        })
    }

    getAccounts() {
        return $.ajax({
            url: this.baseUrl + `getAccounts`,
            method: 'GET',
            headers: { "Authorization": this.accessToken }
        })
    }

    addShopAccount(account) {
        return $.ajax({
            url: this.baseUrl + `addShopAccount`,
            method: 'POST',
            headers: { "Authorization": this.accessToken },
            data: JSON.stringify(account),
            dataType: 'json',
            contentType: 'application/json'
        })
    }

    deleteAccount(accountId) {
        return $.ajax({
            url: this.baseUrl + `deleteAccount/${accountId}`,
            method: 'DELETE',
            headers: { "Authorization": this.accessToken },
        });
    }
}