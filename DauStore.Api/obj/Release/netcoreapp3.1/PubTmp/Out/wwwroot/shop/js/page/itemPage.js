inputSearch = document.querySelector('#inputSearch');
btnRefresh = document.querySelector('#btnRefresh');
formCategory = $('.form-category');
closeCategory = document.querySelector('#closeCategory');
formData = $('.form-data');
btnAddItem = document.querySelector('#btnAddItem');
closeFormData = document.querySelector('#closeFormData');
btnCategory = document.querySelector('#btnCategory');
tagItems = document.querySelectorAll('.tag-item');
btnUpdateCategory = document.querySelector('#btnUpdateCategory');
btnUploadMedia = document.querySelector('#btnUploadMedia');
inputMedia = document.querySelector('#inputMedia');
listMedia = document.querySelector('.list-media');
listDesctiptionCategory = document.querySelector('.list-category-description .my-list');
listDescriptionFeatureForm = document.querySelector('.list-description-feature');


var firebaseConfig = {
    apiKey: "AIzaSyBXYkViqVCVG80SYe2ENZwHGiFnVHz_aq4",
    authDomain: "daustorehieunv.firebaseapp.com",
    projectId: "daustorehieunv",
    storageBucket: "daustorehieunv.appspot.com",
    messagingSenderId: "1039383255491",
    appId: "1:1039383255491:web:24a8b5f93e5e6c5aa78576",
    measurementId: "G-R8NSKY9MV4"
};
// Initialize Firebase
firebase.initializeApp(firebaseConfig);


itemPage = null;
window.onload = () => {
    itemPage = new ItemPage();

    Validator({
        form: '.form-data',
        rules: [
            Validator.isRequired('#valueItemName'),
            Validator.isRequired('#valueRealPrice'),
            Validator.isRequired('#valueSaleRate'),
            Validator.isRequired('#valueMedias'),
        ],
        submitSelector: '#btnUpdateUserInfo',
        onSubmit: itemPage.formItemOnSubmit
    });
}


class ItemPage extends Base {
    constructor() {
        super();
        this.categoryCode = "";
        this.formCategory = {};
        this.formCategoryMode = '';
        this.formItem = {};
        this.formItemMode = '';
        this.listCategory = [];
        this.newCode = '';
        this.itemListDescription = {
            listFeature: [],
            listFeatureValue: []
        }
        this.initEvent();
        this.loadListItem(null, null, 0, 10);
        this.renderTreeList();
    }

    initEvent() {

        inputSearch.addEventListener('keypress', e => {
            if (e.key === 'Enter') {
                this.loadListItem(inputSearch.value, this.categoryCode, this.index, this.count);
            }
        });

        btnRefresh.addEventListener('click', () => {
            window.location.reload();
        });

        btnCategory.addEventListener('click', () => {
            formCategory.show();
        });

        closeCategory.addEventListener('click', () => {
            formCategory.hide();
        });

        btnAddItem.addEventListener('click', () => {

            // reset lại form
            document.querySelector('#valueItemName').value = '';
            document.querySelector('#valueItemCode').value = '';
            document.querySelector('#valueRealPrice').value = '';
            document.querySelector('#valueSaleRate').value = '';
            document.querySelector('#valueCategoryCode .category-main').innerHTML = 'Vui lòng chọn danh mục';
            document.querySelector('#valueCategoryCode').setAttribute('value', '');
            document.querySelector('#valueMedias').setAttribute('value', "");
            listMedia.innerHTML = '';
            document.querySelector('#valueTag').setAttribute('value', "");
            this.setValueTag('');

            // đưa form về dạng chưa cảnh báo
            let listValueSelector = ['#valueItemName', '#valueRealPrice', '#valueSaleRate', '#valueMedias'];
            listValueSelector.forEach(selector => {
                let element = document.querySelector(selector);
                element.classList.remove('validate-field');
                element.title = '';
            })
            this.API.getNewItemCode().done(res => {
                document.querySelector('#valueItemCode').value = res.data;
                this.newCode = res.data;
            });
            $("#updateInStock").hide();
            formData.show();
            document.querySelector('#valueItemName').focus();
            this.formItemMode = 'add';
        });

        closeFormData.addEventListener('click', () => {
            formData.hide();
        });

        tagItems.forEach(item => {
            item.addEventListener('click', () => {
                if (item.classList.contains('checked')) {
                    item.classList.remove('checked');
                } else {
                    item.classList.add('checked');
                }

                let value = '';
                tagItems.forEach(tag => {
                    if (tag.classList.contains('checked')) {
                        value += `${tag.getAttribute('data')} `;
                    };
                });
                value = value.trim();
                document.querySelector('#valueTag').setAttribute('value', value);
            })
        });

        btnUpdateCategory.addEventListener('click', () => {
            this.formCategory.categoryName = document.querySelector('#valueCategoryName').value;
            if ((!this.formCategory.categoryName)) {
                showToastMessenger('danger', "Chưa đủ dữ liệu để cập nhật danh mục!")
            } else {
                if (this.formCategoryMode === 'add') {

                    var popupBtns = [{ text: "Hủy", enable: true }, { text: "Thêm", enable: true }, { text: "Xóa", enable: false }]
                    var btns = showPopupDialog("Thông báo",
                        `Xác nhận thêm danh mục <b>${this.formCategory.categoryName}</b> vào danh mục có mã <b>${this.formCategory.categoryCode}</b> ?`, popupBtns);
                    btns[0].addEventListener('click', () => {
                        hidePopupDialog();
                    });
                    btns[1].addEventListener('click', () => {
                        this.API.addCategory({
                            categoryName: this.formCategory.categoryName,
                            parentCode: (this.formCategory.categoryCode === '') ? 'xxx' : this.formCategory.categoryCode,
                            categoryListDescription: this.getListDescriptionCategory()
                        }).done(res => {
                            showToastMessenger('success', "Thêm mới thành công một danh mục!");
                            hidePopupDialog();
                            this.renderTreeList();
                            document.querySelector('#valueCategoryName').value = '';
                        })
                    });
                } else {
                    var popupBtns = [{ text: "Hủy", enable: true }, { text: "Cập nhật", enable: true }, { text: "Xóa", enable: false }]
                    var btns = showPopupDialog("Thông báo",
                        `Xác nhận sửa danh mục mã <b>${this.formCategory.categoryCode}</b> tên thành <b>${this.formCategory.categoryName}</b> ?`, popupBtns);
                    btns[0].addEventListener('click', () => {
                        hidePopupDialog();
                    });
                    btns[1].addEventListener('click', () => {
                        this.API.updateCategory(this.formCategory.categoryId, this.formCategory.categoryName, this.getListDescriptionCategory()).done(res => {
                            showToastMessenger('success', "Chỉnh sửa thành công danh mục!");
                            hidePopupDialog();
                            this.renderTreeList();
                            document.querySelector('#valueCategoryName').value = '';
                        })
                    });
                }
            }
        });

        btnUploadMedia.addEventListener('click', () => {
            let seft = this;
            var newFileName = this.getNewFileName();
            const file = inputMedia.files[0];
            if (file) {
                const storageRef = firebase.storage().ref();
                const final = storageRef.child(`${newFileName}.png`);
                const task = final.put(file);
                // showLoader();
                task.on('state_changed',
                    function progress(progress) {

                        console.log(progress.bytesTransferred / progress.totalBytes * 100)
                    },
                    function error(err) {
                        console.log(err);
                        showToastMessenger('danger', err);
                    },
                    function completed() {
                        inputMedia.value = '';
                        if (document.querySelector('#valueMedias').getAttribute('value')) {
                            let newMedias = document.querySelector('#valueMedias').getAttribute('value').trim() + ` ${newFileName}`;
                            document.querySelector('#valueMedias').setAttribute('value', newMedias);
                        } else {
                            document.querySelector('#valueMedias').setAttribute('value', newFileName);
                        };
                        showToastMessenger('success', "Tải lên thành công media với name: " + newFileName);
                        // hideLoader();
                        let newImg = parseHTML(`<img src="${getMediaUrl(newFileName)}" alt="${newFileName}">`);
                        listMedia.append(newImg);
                        seft.initEventMedia();
                        document.querySelector('#valueMedias').classList.remove('validate-field');
                    }
                )
            } else {
                showToastMessenger('danger', "Vui lòng chọn file từ máy trước!")
            }
        });

        document.querySelector('.mybtn.add-description').addEventListener('click', () => {
            let newItem = parseHTML(`<div class="category-description-item">
                                        <input type="text" value="">
                                        <div class="mybtn remove-description"><i class="fas fa-minus"></i></div>
                                    </div>`);
            listDesctiptionCategory.append(newItem);
            newItem.querySelector('input').focus();
            this.initEventListDescription();
        });


        document.querySelector('#valueRealPrice').addEventListener('change', (e) => {
            let saleRate = document.querySelector('#valueSaleRate').value;
            if (saleRate) {
                document.querySelector('#valueSalePrice').value = parseInt(Number(e.target.value) * (1 - Number(saleRate) * 0.01));
            }
        });

        document.querySelector('#valueSaleRate').addEventListener('change', (e) => {
            let realPrice = document.querySelector('#valueRealPrice').value;
            if (realPrice) {
                document.querySelector('#valueSalePrice').value = parseInt(Number(realPrice) * (1 - Number(e.target.value) * 0.01));
            }
        });

        document
            .querySelector("#updateInStock")
            .addEventListener("click", () => {
                document.querySelector(".form-update-instock").setAttribute("show", "show");
                document.querySelector('#inputNewQuantity').value = document.querySelector('#inputOldQuantity').value;
                document.querySelector('#inputChangeOfQuantity').value = 0;
            });

        document.querySelector('#closeFormUpdateInstock').addEventListener('click', () => {
            document.querySelector(".form-update-instock").setAttribute("show", "hide");
        });

        document.querySelector('#btnUpdateInstock').addEventListener('click', () => {
            if (document.querySelector('#inputChangeOfQuantity').value != 0) {
                let changeNumber = document.querySelector("#inputChangeOfQuantity").value;
                let itemId = this.formItem.itemId;
                this.API.changeItemInStock(itemId, changeNumber).done(res => {
                    document.querySelector(".form-update-instock").setAttribute("show", "hide");
                    showToastMessenger('success', "Cập nhật số lượng sản phẩm thành công");
                }).fail(err => {
                    showToastMessenger('danger', "Cập nhật số lượng sản phẩm thất bại");
                })
            } else {
                showToastMessenger('success', "Không có gì thay đổi cả");
                document.querySelector(".form-update-instock").setAttribute("show", "hide");
            }
        });

        document.querySelector('#inputChangeOfQuantity').addEventListener('change', (e) => {
            let changeNumber = parseInt(e.target.value);
            let minChange = parseInt(e.target.getAttribute('min'));
            if (changeNumber < minChange) {
                changeNumber = minChange;
                e.target.value = minChange;
            }
            document.querySelector('#inputNewQuantity').value = Number(document.querySelector('#inputOldQuantity').value) + changeNumber;
        });

        document.querySelector('#inputNewQuantity').addEventListener('change', (e) => {
            if (Number(e.target.value) < 0) {
                e.target.value = 0;
            }
            document.querySelector('#inputChangeOfQuantity').value = Number(e.target.value) - Number(document.querySelector('#inputOldQuantity').value);
        });
    }

    initEventMedia() {
        document.querySelectorAll('.list-media img').forEach(img => {
            img.addEventListener('dblclick', () => {
                let medias = listMedia.getAttribute('value');
                medias = medias.trim().split(' ');
                var popupBtns = [{ text: "Đóng", enable: true }, { text: "Đặt làm avatar", enable: true }, { text: "Xóa", enable: true }]
                var btns = showPopupDialog("Thông báo", `Bạn muốn làm gì với media này?`, popupBtns);
                btns[0].addEventListener('click', () => {
                    hidePopupDialog();
                });
                btns[1].addEventListener('click', () => {

                    medias = [
                        medias.find(e => e === img.alt),
                        ...medias.filter(e => e !== img.alt)
                    ];
                    medias = medias.join(' ');
                    this.loadListMedia(medias);
                    listMedia.setAttribute('value', medias);
                    hidePopupDialog();
                });
                btns[2].addEventListener('click', () => {
                    const storageRef1 = firebase.storage().ref()
                    const final1 = storageRef1.child(`${img.alt}.png`);

                    final1.delete().then(() => {
                        showToastMessenger('success', "Xóa thành công media!");
                        medias = medias.filter(e => e !== img.alt);
                        medias = medias.join(' ');
                        this.loadListMedia(medias);
                        listMedia.setAttribute('value', medias);
                        hidePopupDialog();
                    }).catch(err => {
                        showToastMessenger('danger', "err");
                    })
                });
            })
        });
    }

    getNewFileName() {
        let medias = document.querySelector('#valueMedias').getAttribute('value');
        if (medias) {
            let maxNumber = 0;
            medias.trim().split(' ').forEach(mediaName => {
                let mediaNumber = Number(mediaName.split('_')[1]);
                maxNumber = (maxNumber < mediaNumber) ? mediaNumber : maxNumber;
            });

            return `${this.newCode}_${(maxNumber + 1)}`;
        } else {
            return `${this.newCode}_${0}`;
        }
    }

    loadListMedia(medias) {
        listMedia.innerHTML = '';
        medias = medias.trim().split(' ').forEach(media => {
            let img = parseHTML(`<img src="${getMediaUrl(media)}" alt="${media}">`);
            listMedia.append(img);
        });
        this.initEventMedia();
    }

    loadListItem(searchTerms, categoryCode, index, count) {
        console.log(JSON.stringify({ searchTerms, categoryCode, index, count }));
        this.API.getItems(categoryCode, searchTerms, null, 1, index, count).done(res => {
            loadTable(listColums.Items, res.data.data, this.index + 1);
            this.total = res.data.total;
            this.initEventTable();
            this.reloadPagingInfo();
        }).fail(err => {
            showToastMessenger('danger', "Có lỗi");
        })
    }

    renderTreeList() {
        document.querySelector('#category').innerHTML = '';
        document.querySelector('#categoryx').innerHTML = '';
        document.querySelector('#category_form').innerHTML = '';
        this.API.getCategorys().done(res => {
            this.listCategory = res.data;
            loadListCategory(res.data);
            loadListCategoryx(res.data);
            loadListCategoryForm(res.data);
            this.initEventTreeList();
            localStorage.setItem('category', JSON.stringify(res.data));
        }).fail(err => {
            showToastMessenger('danger', "Có lỗi");
        });
    }

    tableRowOnDBClick(item, thisTr) {
        var popupBtns = [{ text: "Đóng", enable: true }, { text: "Cập nhật", enable: true }, { text: "Xóa", enable: true }]
        var btns = showPopupDialog("Thông báo", `Bạn muốn làm gì với mặt hàng <b>${item.itemName}</b>`, popupBtns);
        btns[0].addEventListener('click', () => {
            hidePopupDialog();
        });
        btns[1].addEventListener('click', () => {
            console.log(item);
            this.formItemMode = 'update';
            // bind dữ liệu item lên form;
            this.newCode = item.itemCode;
            this.formItem.itemId = item.itemId;
            document.querySelector('#valueItemName').value = item.itemName;
            document.querySelector('#valueItemCode').value = item.itemCode;
            document.querySelector('#valueRealPrice').value = item.realPrice;
            document.querySelector('#valueSaleRate').value = item.saleRate;
            document.querySelector('#valueCategoryCode .category-main').innerHTML = item.categoryName;
            document.querySelector('#valueCategoryCode').setAttribute('value', item.categoryCode);
            document.querySelector('#valueSalePrice').value = parseInt(item.realPrice * (1 - item.saleRate * 0.01));
            document.querySelector('#inputOldQuantity').value = item.inStock;
            document.querySelector('#inputChangeOfQuantity').setAttribute('min', -item.inStock);

            let thisItemCategory = JSON.parse(localStorage.getItem('category')).find(cate => cate.categoryCode === item.categoryCode);
            this.loadListDescriptionFeatureForm(thisItemCategory ? thisItemCategory.categoryListDescription : [], item.listDescription);
            this.itemListDescription.listFeature = thisItemCategory.categoryListDescription;
            this.itemListDescription.listFeatureValue = item.listDescription;
            console.log(this.itemListDescription);

            document.querySelector('#valueMedias').setAttribute('value', item.medias);
            this.loadListMedia(item.medias);
            document.querySelector('#valueTag').setAttribute('value', item.tag);
            this.setValueTag(item.tag);
            $("#updateInStock").show();
            formData.show();
            hidePopupDialog();
        });
        btns[2].addEventListener('click', () => {
            hidePopupDialog();
            var popupBtns = [{ text: "Không", enable: true }, { text: "Xóa", enable: true }, { text: "Xóa", enable: false }]
            var btns = showPopupDialog("Thông báo", `Bạn chắc chắn muốn xóa mặt hàng <b>${item.itemName}</b>`, popupBtns);
            btns[0].addEventListener('click', () => {
                hidePopupDialog();
            });
            btns[1].addEventListener('click', () => {
                this.API.deleteItem(item.itemId).done(res => {
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
        this.loadListItem(inputSearch.value, this.categoryCode, this.index, this.count);
    }
    prePage() {
        this.loadListItem(inputSearch.value, this.categoryCode, this.index, this.count);
    }
    nextPage() {
        if (this.index >= this.total) {
            this.index -= this.count
            showToastMessenger('danger', "Đã đến trang cuối!")
        } else {
            this.loadListItem(inputSearch.value, this.categoryCode, this.index, this.count);
        }
    }

    setValueTag(tag) {
        console.log(tag);
        document.querySelectorAll('.tag-item').forEach(tagItem => {
            if (tag.includes(tagItem.getAttribute('data'))) {
                tagItem.classList.add('checked');
            } else {
                tagItem.classList.remove('checked');
            }
        });
    }

    initEventTreeList() {
        // chọn category trong filter
        document.querySelectorAll('.function-bar div.tree-nav__item-title').forEach(element => {
            element.addEventListener('click', () => {
                document.querySelectorAll('.function-bar div.tree-nav__item-title').forEach(e => {
                    e.classList.remove('active');
                });
                element.classList.add('active');
                document.querySelector('.function-bar .category-main').innerHTML = element.getAttribute('name');
                this.categoryCode = element.getAttribute('code');
                this.loadListItem(inputSearch.value, this.categoryCode, this.index, this.count);


            });
        });

        //chọn category trong form
        document.querySelectorAll('.form-data div.tree-nav__item-title').forEach(element => {
            element.addEventListener('click', () => {
                document.querySelector('#valueCategoryCode').setAttribute('value', element.getAttribute('code'));
                document.querySelector('#valueCategoryCode .category-main').innerHTML = element.getAttribute('name');
                let listFeature = element.getAttribute('categoryListDescription').split('XXX');
                this.loadListDescriptionFeatureForm(listFeature, null);

                // lọc và lấy những giá trị cũ đã có
                listFeature.forEach((feature, index) => {
                    for (let i = 0; i < this.itemListDescription.listFeature.length; i++) {
                        let _feature = this.itemListDescription.listFeature[i];
                        if (feature == _feature) {
                            document.querySelectorAll('.list-description-feature input')[index].value =
                                this.itemListDescription.listFeatureValue[i]
                        } else {
                            continue;
                        }
                    }
                });

                document.querySelectorAll('.form-data div.tree-nav__item-title').forEach(e => {
                    e.classList.remove('active');
                })
                element.classList.add('active');
            });
        });

        document.querySelectorAll('.tool-option.add').forEach(element => {
            element.addEventListener('click', () => {
                let value = element.parentElement.parentElement.getAttribute('code');
                let parentName = element.parentElement.parentElement.getAttribute('name');
                let listDescription = element.parentElement.parentElement.getAttribute('categoryListDescription').split('XXX');
                console.log(value);
                this.formCategoryMode = 'add';
                this.formCategory.categoryCode = value;
                document.querySelector('#valueCategoryName').value = '';
                document.querySelector('#valueCategoryName').focus();
                document.querySelector('#valueCategoryParent').value = parentName;
                this.loadCategoryListDescription(listDescription);
            })
        });

        document.querySelectorAll('.tool-option.edit').forEach(element => {
            element.addEventListener('click', () => {
                this.formCategory.categoryCode = element.parentElement.parentElement.getAttribute('code');
                this.formCategory.categoryId = element.parentElement.parentElement.getAttribute('categoryId');
                let listDescription = element.parentElement.parentElement.getAttribute('categoryListDescription').split('XXX');
                this.formCategoryMode = 'edit';
                document.querySelector('#valueCategoryName').value = element.parentElement.parentElement.getAttribute('name');
                document.querySelector('#valueCategoryName').focus();
                document.querySelector('#valueCategoryParent').value = "";
                this.loadCategoryListDescription(listDescription);
            })
        });

        document.querySelectorAll('.tool-option.delete').forEach(element => {
            element.addEventListener('click', () => {
                let id = element.parentElement.parentElement.getAttribute('categoryId');
                let code = element.parentElement.parentElement.getAttribute('code');
                var popupBtns = [{ text: "Đóng", enable: true }, { text: "Xóa", enable: true }, { text: "Xóa", enable: false }]
                var btns = showPopupDialog("Thông báo", `Bạn muốn xóa danh mục có mã <b>${code}</b> ?`, popupBtns);
                btns[0].addEventListener('click', () => {
                    hidePopupDialog();
                });
                btns[1].addEventListener('click', () => {
                    this.API.deleteCategory(id).done(res => {
                        showToastMessenger('success', "Xóa thành công!");
                        hidePopupDialog();
                        this.renderTreeList();
                    }).fail(err => {
                        showToastMessenger('danger', "Lỗi");
                    });
                });
            })
        });
    }

    formItemOnSubmit() {
        itemPage.formItem.itemName = document.querySelector('#valueItemName').value;
        itemPage.formItem.itemCode = document.querySelector('#valueItemCode').value;
        itemPage.formItem.realPrice = Number(document.querySelector('#valueRealPrice').value);
        itemPage.formItem.saleRate = Number(document.querySelector('#valueSaleRate').value);
        itemPage.formItem.tag = document.querySelector('#valueTag').getAttribute('value');
        itemPage.formItem.categoryCode = document.querySelector('#valueCategoryCode').getAttribute('value');
        itemPage.formItem.medias = document.querySelector('#valueMedias').getAttribute('value');
        itemPage.formItem.listDescription = itemPage.getListDescriptionFeature();

        console.log(itemPage.formItem);

        if (itemPage.formItemMode === 'add') {
            itemPage.API.addItem(itemPage.formItem).done(res => {
                showToastMessenger('success', "Thêm mới mặt hàng thành công!");
                formData.hide();
                itemPage.loadListItem(inputSearch.value, itemPage.categoryCode, itemPage.index, itemPage.count);
            }).fail(err => {
                console.log(err);
                showToastMessenger('danger', "Có lỗi, vui lòng thử lại!");
            });
        } else {
            itemPage.API.updateItem(itemPage.formItem, itemPage.formItem.itemId).done(res => {
                showToastMessenger('success', "Cập nhật mặt hàng thành công!");
                formData.hide();
                itemPage.loadListItem(inputSearch.value, itemPage.categoryCode, itemPage.index, itemPage.count);
            }).fail(err => {
                showToastMessenger('danger', "Có lỗi, vui lòng thử lại!");
            })
        }
    }

    loadCategoryListDescription(categoryListDescription) {
        listDesctiptionCategory.innerHTML = '';
        categoryListDescription.forEach(i => {
            var itemDescription = parseHTML(
                `<div class="category-description-item">
                        <input type="text" value="${i}">
                        <div class="mybtn remove-description"><i class="fas fa-minus"></i></div>
                </div>`);
            listDesctiptionCategory.append(itemDescription);
        });
        this.initEventListDescription();
    }

    initEventListDescription() {
        document.querySelectorAll('.mybtn.remove-description').forEach(element => {
            element.addEventListener('click', () => {
                element.parentElement.remove();
            });
        });

        document.querySelectorAll('.category-description-item input').forEach(input => {
            input.addEventListener("keyup", function (event) {
                if (event.keyCode === 13) {
                    event.preventDefault();
                    document.querySelector('.mybtn.add-description').click();
                }
            });
        });

    };

    getListDescriptionCategory() {
        let listDes = [];
        listDesctiptionCategory.querySelectorAll('.category-description-item input').forEach(input => {
            if (input.value.trim())
                listDes.push(input.value.trim());
        });
        return listDes;
    }

    loadListDescriptionFeatureForm(listFeature, listFeatureValue) {
        listDescriptionFeatureForm.innerHTML = '';
        if (listFeatureValue) {
            for (let i = 0; i < listFeature.length; i++) {
                let featureInput = parseHTML(`<div class="form-item divide-1-row">
                                            <div>${listFeature[i]}: </div>
                                            <input type="text" class="input" value="${listFeatureValue[i]}">
                                        </div>`);
                listDescriptionFeatureForm.append(featureInput);
            }
        } else {
            listFeature.forEach(feature => {
                let featureInput = parseHTML(`<div class="form-item divide-1-row">
                                            <div>${feature}: </div>
                                            <input type="text" class="input" value="">
                                        </div>`);
                listDescriptionFeatureForm.append(featureInput);
            });
        }
    }

    getListDescriptionFeature() {
        let listFeature = [];
        document.querySelectorAll('.list-description-feature input').forEach(input => {
            if (input.value.trim()) {
                listFeature.push(input.value.trim());
            }
        });
        return listFeature;
    }
}
