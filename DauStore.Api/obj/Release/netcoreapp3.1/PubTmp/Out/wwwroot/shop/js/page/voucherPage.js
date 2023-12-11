inputSearch = document.querySelector('#inputSearch');
btnRefresh = document.querySelector('#btnRefresh');
btnAddVoucher = document.querySelector('#btnAddVoucher');
closeFormData = document.querySelector('#closeFormData');
btnRenderDescription = document.querySelector('#renderDescription');
formData = $('.form-data');

voucherPage = null;
window.onload = () => {
    voucherPage = new VoucherPage();

    Validator({
        form: '.form-data',
        rules: [
            Validator.isRequired('#valueVoucherCode'),
            Validator.isRequired('#valueDescription'),
            Validator.isRequired('#valueMinTotal'),
            Validator.isRequired('#valueQuota'),
            Validator.isRequired('#valueDateExpired'),
        ],
        submitSelector: '#btnUpdateVoucher',
        onSubmit: voucherPage.formVoucherOnSubmit
    });
}

class VoucherPage extends Base {
    constructor() {
        super();
        this.formVoucherMode = '';
        this.formVoucher = {};
        this.isCanUse = 0;
        this.loadListVoucher('', 0, 0, 10);
        this.initEvent();
    }

    initEvent() {

        btnAddVoucher.addEventListener('click', () => {
            // reset lại form
            document.querySelector('#valueVoucherCode').value = '';
            document.querySelector('#valueSaleRate').value = '';
            document.querySelector('#valueMaxNumber').value = '';
            document.querySelector('#valueSaleNumber').value = '';
            document.querySelector('#valueMinTotal').value = '';
            document.querySelector('#valueQuota').value = '';
            document.querySelector('#valueDateExpired').value = '';
            document.querySelector('#valueDescription').value = '';
            let listValueSelector = ['#valueVoucherCode', '#valueMinTotal', '#valueQuota', '#valueDateExpired', '#valueDescription'];
            listValueSelector.forEach(selector => {
                let element = document.querySelector(selector);
                element.classList.remove('validate-field');
                element.title = '';
            })
            formData.show();
            document.querySelector('#valueVoucherCode').focus();
            this.formVoucherMode = 'add';
        });

        closeFormData.addEventListener('click', () => {
            formData.hide();
        });

        document.querySelectorAll('.radio-button').forEach(radioBtn => {
            radioBtn.addEventListener('click', () => {
                let value = document.querySelector('#voucherType').getAttribute('value');
                if (value === 'percent') {
                    document.querySelector('#valueSaleRate').disabled = false;
                    document.querySelector('#valueMaxNumber').disabled = false;
                    document.querySelector('#valueSaleNumber').disabled = true;
                    document.querySelector('#valueSaleNumber').value = '';
                } else {
                    document.querySelector('#valueSaleRate').disabled = true;
                    document.querySelector('#valueMaxNumber').disabled = true;
                    document.querySelector('#valueSaleNumber').disabled = false;
                    document.querySelector('#valueSaleRate').value = '';
                    document.querySelector('#valueMaxNumber').value = '';
                }
            });
        });

        btnRenderDescription.addEventListener('click', () => {
            let voucherType = document.querySelector('#voucherType').getAttribute('value');
            if (voucherType === 'percent') {
                document.querySelector('#valueDescription').value =
                    `Khuyến mãi ${document.querySelector('#valueSaleRate').value}% tối đa ${document.querySelector('#valueMaxNumber').value}đ cho đơn giá tối thiểu ${document.querySelector('#valueMinTotal').value}đ`;
            } else {
                document.querySelector('#valueDescription').value =
                    `Khuyến mãi ${document.querySelector('#valueSaleNumber').value}đ cho đơn giá tối thiểu ${document.querySelector('#valueMinTotal').value}đ`;
            }
        });

        btnRefresh.addEventListener('click', () => {
            window.location.reload();
        });

        document.querySelector('#canuse0').addEventListener('click', () => {
            this.isCanUse = 0;
            this.loadListVoucher(inputSearch.value, this.isCanUse, this.index, this.count);
        });
        document.querySelector('#canuse1').addEventListener('click', () => {
            this.isCanUse = 1;
            this.loadListVoucher(inputSearch.value, this.isCanUse, this.index, this.count);
        });
        document.querySelector('#canuse2').addEventListener('click', () => {
            this.isCanUse = 2;
            this.loadListVoucher(inputSearch.value, this.isCanUse, this.index, this.count);
        });

        inputSearch.addEventListener('keypress', e => {
            if (e.key === 'Enter') {
                this.loadListVoucher(inputSearch.value, this.isCanUse, this.index, this.count);
            }
        });
    }

    loadListVoucher(searchTerms, canuseState, index, count) {
        this.API.getVouchers(index, count, searchTerms, canuseState).done(res => {
            loadTable(listColums.Vouchers, res.data.data, index + 1);
            this.total = res.data.total;
            this.initEventTable();
            this.reloadPagingInfo();
        }).fail(err => {
            showToastMessenger('danger', "Có lỗi");
        })
    };

    tableRowOnDBClick(voucher, thisTr) {
        var popupBtns = [{ text: "Đóng", enable: true }, { text: "Cập nhật", enable: true }, { text: "Xóa", enable: true }]
        var btns = showPopupDialog("Thông báo", `Bạn muốn làm gì với voucher <b>${voucher.voucherCode}</b>`, popupBtns);
        btns[0].addEventListener('click', () => {
            hidePopupDialog();
        });
        btns[1].addEventListener('click', () => {
            this.formVoucherMode = 'update';
            this.formVoucher.voucherId = voucher.voucherId;
            console.log(JSON.stringify(voucher));
            // bind dữ liệu item lên form;
            document.querySelector('#valueVoucherCode').value = voucher.voucherCode;
            document.querySelector('#valueMinTotal').value = voucher.minTotal;
            document.querySelector('#valueQuota').value = voucher.quota;
            document.querySelector('#valueDateExpired').value = voucher.dateExpired;
            document.querySelector('#valueDescription').value = voucher.description;
            document.querySelector('#valueSaleRate').value = !voucher.saleRate ? '' : voucher.saleRate * 100;
            document.querySelector('#valueMaxNumber').value = !voucher.maxNumber ? '' : voucher.maxNumber;
            document.querySelector('#valueSaleNumber').value = !voucher.saleNumber ? '' : voucher.saleNumber;

            debugger;
            if (voucher.saleNumber) {
                document.querySelectorAll('#voucherType .radio-button')[1].click();
            } else {
                document.querySelectorAll('#voucherType .radio-button')[0].click();
            }
            formData.show();
            hidePopupDialog();
        });
        btns[2].addEventListener('click', () => {
            hidePopupDialog();
            var popupBtns = [{ text: "Không", enable: true }, { text: "Xóa", enable: true }, { text: "Xóa", enable: false }]
            var btns = showPopupDialog("Thông báo", `Bạn chắc chắn muốn xóa voucher <b>${voucher.voucherCode}</b>`, popupBtns);
            btns[0].addEventListener('click', () => {
                hidePopupDialog();
            });
            btns[1].addEventListener('click', () => {
                this.API.deleteVoucher(voucher.voucherId).done(res => {
                    showToastMessenger('success', "Xóa thành công");
                    hidePopupDialog();
                    thisTr.remove();
                }).fail(err => {
                    showToastMessenger('danger', err.responseJSON);
                })
            });
        });
    }

    changeCount() {
        this.loadListVoucher(inputSearch.value, this.isCanUse, this.index, this.count);
    }
    prePage() {
        this.loadListVoucher(inputSearch.value, this.isCanUse, this.index, this.count);
    }
    nextPage() {
        if (this.index >= this.total) {
            this.index -= this.count
            showToastMessenger('danger', "Đã đến trang cuối!")
        } else {
            this.loadListVoucher(inputSearch.value, this.isCanUse, this.index, this.count);
        }
    }

    formVoucherOnSubmit() {
        let a = document.querySelector('#valueSaleRate').value;
        let b = document.querySelector('#valueMaxNumber').value;
        let c = document.querySelector('#valueSaleNumber').value;
        console.log(a, b, c);
        if (c || (a && b)) {

            // lấy giá trị từ các input
            voucherPage.formVoucher.voucherCode = document.querySelector('#valueVoucherCode').value;
            voucherPage.formVoucher.saleRate = !document.querySelector('#valueSaleRate').value ? 0 : Number(document.querySelector('#valueSaleRate').value) / 100;
            voucherPage.formVoucher.maxNumber = !document.querySelector('#valueMaxNumber').value ? 0 : Number(document.querySelector('#valueMaxNumber').value);
            voucherPage.formVoucher.saleNumber = !document.querySelector('#valueSaleNumber').value ? 0 : Number(document.querySelector('#valueSaleNumber').value);
            voucherPage.formVoucher.minTotal = Number(document.querySelector('#valueMinTotal').value);
            voucherPage.formVoucher.quota = Number(document.querySelector('#valueQuota').value);
            voucherPage.formVoucher.dateExpired = document.querySelector('#valueDateExpired').value;
            voucherPage.formVoucher.description = document.querySelector('#valueDescription').value;

            console.log(voucherPage.formVoucher);
            if (voucherPage.formVoucherMode == 'add') {
                voucherPage.API.addVoucher(voucherPage.formVoucher).done(res => {
                    showToastMessenger('success', "Thêm mới thành công voucher khuyến mãi!");
                    formData.hide();
                    voucherPage.loadListVoucher(inputSearch.value, this.isCanUse, this.index, this.count);
                }).fail(err => {
                    showToastMessenger('danger', "Có lỗi vui lòng thử lại!")
                });
            } else {
                voucherPage.API.updateVoucher(voucherPage.formVoucher, voucherPage.formVoucher.voucherId).done(res => {
                    showToastMessenger('success', "Cập nhật thành công voucher khuyến mãi!");
                    formData.hide();
                    voucherPage.loadListVoucher(inputSearch.value, this.isCanUse, this.index, this.count);
                }).fail(err => {
                    showToastMessenger('danger', "Có lỗi vui lòng thử lại!")
                })
            }
        } else {
            showToastMessenger("danger", "Giá trị phần trăm | số tiền khuyến mãi chưa được điền!");
        }
    }
}
