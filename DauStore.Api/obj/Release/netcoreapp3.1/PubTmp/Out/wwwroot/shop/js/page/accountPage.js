btnRefresh = document.querySelector('#btnRefresh');
btnAddAccount = document.querySelector('#btnAddAccount');
closeFormData = document.querySelector('#closeFormData');
formData = $('.form-data');

accountPage = null;
window.onload = () => {
    accountPage = new AccountPage();

    Validator({
        form: '.form-data',
        rules: [
            Validator.isRequired('#valueAccountName'),
            Validator.isRequired('#valuePhone'),
            Validator.isRequired('#valuePassword')
        ],
        submitSelector: '#btnAddNewAccount',
        onSubmit: accountPage.formAccountOnSubmit
    });
}

class AccountPage extends Base {
    constructor() {
        super();
        this.formAccountMode = '';
        this.formAccount = {};
        this.loadListAccount();
        this.initEvent();
    }

    initEvent() {

        btnAddAccount.addEventListener('click', () => {
            // reset lại form
            document.querySelector('#valueAccountName').value = '';
            document.querySelector('#valuePhone').value = '';
            document.querySelector('#valueEmail').value = '';
            document.querySelector('#valuePassword').value = '';
            let listValueSelector = ['#valueAccountName', '#valuePhone', '#valuePassword'];
            listValueSelector.forEach(selector => {
                let element = document.querySelector(selector);
                element.classList.remove('validate-field');
                element.title = '';
            })
            formData.show();
            document.querySelector('#valueAccountName').focus();
            this.formAccountMode = 'add';
        });

        closeFormData.addEventListener('click', () => {
            formData.hide();
        });

        document.querySelectorAll('.radio-button').forEach(radioBtn => {
            radioBtn.addEventListener('click', () => {
                let value = document.querySelector('#AccountType').getAttribute('value');
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

        btnRefresh.addEventListener('click', () => {
            window.location.reload();
        });
    }

    loadListAccount() {
        this.API.getAccounts().done(res => {
            console.log(res);
            loadTable(listColums.Accounts, res.data, 1);
            this.total = res.data.total;
            this.initEventTable();
            document.querySelector('#pagingInfo').innerHTML =
                `Hiển thị tất cả bản ghi`;
        }).fail(err => {
            showToastMessenger('danger', "Có lỗi");
        })
    };

    tableRowOnDBClick(Account, thisTr) {
        var popupBtns = [{ text: "Đóng", enable: true }, { text: "Reset Password", enable: true }, { text: "Xóa", enable: true }]
        var btns = showPopupDialog("Thông báo", `Bạn muốn làm gì với Account <b>${Account.phone}</b>`, popupBtns);
        btns[0].addEventListener('click', () => {
            hidePopupDialog();
        });
        btns[1].addEventListener('click', () => {
            hidePopupDialog();
            var popupBtns = [{ text: "Không", enable: true }, { text: "Reset", enable: true }, { text: "Xóa", enable: false }]
            var btns = showPopupDialog("Thông báo", `Bạn chắc chắn muốn đặt mật khẩu tài khoản <b>${Account.phone}</b> thành 123456?`, popupBtns);
            btns[0].addEventListener('click', () => {
                hidePopupDialog();
            });
            btns[1].addEventListener('click', () => {
                setTimeout(() => {
                    showToastMessenger('success', "Reset thành công!");
                    hidePopupDialog();
                }, 1000);
            });
        });
        btns[2].addEventListener('click', () => {
            hidePopupDialog();
            var popupBtns = [{ text: "Không", enable: true }, { text: "Xóa", enable: true }, { text: "Xóa", enable: false }]
            var btns = showPopupDialog("Thông báo", `Bạn chắc chắn muốn xóa tài khoản <b>${Account.phone}</b>`, popupBtns);
            btns[0].addEventListener('click', () => {
                hidePopupDialog();
            });
            btns[1].addEventListener('click', () => {
                this.API.deleteAccount(Account.accountId).done(res => {
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
    }
    prePage() {
    }
    nextPage() {
    }

    formAccountOnSubmit() {
        // lấy giá trị từ các input
        accountPage.formAccount.accountName = document.querySelector('#valueAccountName').value;
        accountPage.formAccount.phone = document.querySelector('#valuePhone').value;
        accountPage.formAccount.email = document.querySelector('#valueEmail').value;
        accountPage.formAccount.password = document.querySelector('#valuePassword').value;

        console.log(accountPage.formAccount);
        if (accountPage.formAccountMode == 'add') {
            accountPage.API.addShopAccount(accountPage.formAccount).done(res => {
                showToastMessenger('success', "Thêm mới thành công tài khoản!");
                formData.hide();
                accountPage.loadListAccount();
            }).fail(err => {
                showToastMessenger('danger', "Có lỗi vui lòng thử lại!")
            });
        } else {
            accountPage.API.updateAccount(accountPage.formAccount, accountPage.formAccount.AccountId).done(res => {
                showToastMessenger('success', "Cập nhật thành công Account khuyến mãi!");
                formData.hide();
                accountPage.loadListAccount(inputSearch.value, this.isCanUse, this.index, this.count);
            }).fail(err => {
                showToastMessenger('danger', "Có lỗi vui lòng thử lại!")
            })
        }
    }
}
