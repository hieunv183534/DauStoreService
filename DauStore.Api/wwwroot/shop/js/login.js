API = null;
inputPhone = document.querySelector('#valuePhoneNumber');
inputPassword = document.querySelector('#valuePassword');
btnLogin = document.querySelector('#btnLogin');
window.onload = () => {
    API = new BaseApi();
    initEvent();
}

function initEvent() {
    btnLogin.addEventListener('click', () => {
        if(inputPhone.value === ""){
            showToastMessenger('danger',  "Bạn chưa nhập số điện thoại!");
            inputPhone.focus();
        }else if(inputPassword.value === ""){
            showToastMessenger('danger',  "Bạn chưa nhập mật khẩu!");
            inputPassword.focus();
        }

        API.login(inputPhone.value, inputPassword.value).done(res => {
            sessionStorage.setItem('access_token', res.data.token);
            window.location.href = "./page/home.html";
        }).fail(err => {
            showToastMessenger('danger',  "Thông tin đăng nhập không đúng!")
        })
    })
}