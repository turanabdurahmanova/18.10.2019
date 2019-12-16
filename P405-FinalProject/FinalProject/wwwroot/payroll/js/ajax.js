$(document).ready(function () {

    //@* ------ company delete ----------------------------------*@

    let companyId, deletedCompany;
    $(document).on("click", ".companyDelete", function (e) {
        companyId = $(this).data("id");
        deletedCompany = $(this).parents();
        if (companyId) {
            $.ajax({
                url: "/PayrollAdmin/Company/Delete",
                method: "Get",
                data: { id: companyId },
                success: function (respons) {
                    if (respons.message == 200) {
                        $("#companyDeleteName").text(respons.companyDb.name);
                        $('#companyDelete').modal('show')
                    }
                },
                error: function (xhr, status, error) {
                    console.log(xhr.status);
                    console.log(status);
                    console.log(error);
                }
            });
        }
    });

    $(document).on("click", ".companyDeleteSave", function (e) {
        e.preventDefault();
        var ajax = ({
            url: "/PayrollAdmin/Company/DeletePost",
            method: "POST",
            async: true,
            data: { id: companyId },
            success: function (respons) {
                if (respons.message == 200) {
                    $(deletedCompany).parents("div.col-xl-3").remove();
                }
            }
        });

        var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
        if (antiForgeryToken) {
            ajax.headers = {};
            ajax.headers["X-XSRF-Token"] = antiForgeryToken;
        };

        $.ajax(ajax);
    });

    //@* ------ company search ----------------------------------*@

    function addNewCompany(datas) {
        var maindiv = $("#divCompany").clone();
        $("div.companyClone").html(maindiv);
        for (var data of datas) {
            var div = $("#divCompany").clone();
            div.removeClass("d-none");
            div.removeAttr('id');
            div.find("h3").text(data.name);
            div.find("ul li:nth-child(1) a").attr({ href: "/PayrollAdmin/Company/Edit/" + data.id });
            div.find("ul li:nth-child(2) button").attr("data-id", data.id);
            div.find("ul li:nth-child(3) a").attr({ href: "/PayrollAdmin/Company/Details/" + data.id });
            $("div.companyClone").append(div);
        }
    }

    $(document).on("click", ".searchCompany", function (e) {
        e.preventDefault();
        var name = $("#companyName").val();
        $.ajax({
            url: "/PayrollAdmin/Ajax/CompanySearch",
            method: "Get",
            data: { name: name },
            success: function (res) {
                if (res.status == 200) {
                    $("#companyNotFoundError").html("");
                    addNewCompany(res.data);
                }
                else if (res.status == 400) {
                    $("#companyNotFoundError").html("");
                    var span = document.createElement('span');
                    span.textContent = "Belə Şirkət yoxdur !!!";
                    $("#companyNotFoundError").append(span);
                    addNewCompany(res.data);
                }
            }
        });
    })

    //@*--------departament create----------------------------------------------------------------------------------------------------------*@

    $(document).on("click", ".departamentCreate", function (e) {
        $("#departamentCreate").modal("show");
    });

    $(document).on("click", ".departamentCreateSave", function (e) {
        let departamentName = $("#departmantNameCreate").val();
        console.log(departamentName);
        if (departamentName) {
            var ajax = ({
                url: "/PayrollAdmin/Departament/Create",
                data: { name: departamentName },
                method: "Post",
                success: function (respons) {
                    if (respons.message == 200) {
                        alert("Əməliyyat uğurludur: Departament əlavə edildi...")
                    }
                }
            })
        }

        var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
        if (antiForgeryToken) {
            ajax.headers = {};
            ajax.headers["X-XSRF-Token"] = antiForgeryToken;
        };

        $.ajax(ajax);
    });

    //@-------- departament edit -------------------------------------------------------------------------------------------------------------*@

    let departmantId;
    $(document).on("click", ".departamentEdit", function (e) {
        departmantId = $(this).data("id");
        $.ajax({
            url: "/PayrollAdmin/Departament/Edit",
            data: { id: departmantId },
            type: "Get",
            success: function (respons) {
                if (respons.message == 200) {
                    $("#departmantNameUpdate").val(respons.departmantDb.name);
                    $('#departamentEdit').modal('show')
                }
            }
        });
    });

    $(document).on("click", ".departamentEditSave", function (e) {
        e.preventDefault();
        let name = $("#departmantNameUpdate").val();
        if (name && departmantId) {
            var ajax = {
                url: "/PayrollAdmin/Departament/EditPost",
                data: { id: departmantId, name: name },
                type: "Post",
                success: function (respons) {
                    if (respons.message == 200) {
                        alert("Əməliyyat uğurludur: Departamentin adı redaktə edildi...")
                    }
                }
            }

            var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
            if (antiForgeryToken) {
                ajax.headers = {};
                ajax.headers["X-XSRF-Token"] = antiForgeryToken;
            };
            $.ajax(ajax);
        }
    });

    //@*--------- departament delete ----------------------------------------------------------------------------------------------------*@

    let deletedDepartament
    $(document).on("click", ".departamentDelete", function (e) {
        departmantId = $(this).data("id");
        deletedDepartament = $(this).parents();
        $.ajax({
            url: "/PayrollAdmin/Departament/Delete",
            method: "Get",
            data: { id: departmantId },
            async: true,
            success: function (res) {
                if (res.message == 200) {
                    $("#departmantNameDelete").text(res.departmantDb.name);
                    $('#departamentDelete').modal('show')
                }
            }
        });
    });

    $(document).on("click", ".departamentDeletedSave", function (e) {
        e.preventDefault();
        if (departmantId) {
            var ajax = ({
                url: "/PayrollAdmin/Departament/DeletePost",
                method: "POST",
                async: true,
                data: { id: departmantId },
                success: function (res) {
                    if (res.message == 200) {
                        $(deletedDepartament).parents("div.col-xl-3").remove();
                    }
                }
            });

            var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
            if (antiForgeryToken) {
                ajax.headers = {};
                ajax.headers["X-XSRF-Token"] = antiForgeryToken;
            };

            $.ajax(ajax);
        }
    });

    //@* ------ departament search ------------------------------*@

    function addNewDepartament(datas) {
        var maindiv = $("#divDepartament").clone();
        $("div.divDepartamentClone").html(maindiv);
        for (var data of datas) {
            var div = $("#divDepartament").clone();
            div.removeClass("d-none");
            div.removeAttr('id');
            div.find("h4").text(data.name);
            div.find("ul li:nth-child(1) button").attr("data-id", data.id);
            div.find("ul li:nth-child(2) button").attr("data-id", data.id);
            $("div.divDepartamentClone").append(div);
        }
    }

    $(document).on("click", ".searchDepartament", function (e) {
        e.preventDefault();
        var name = $("#departamentName").val();
        $.ajax({
            url: "/PayrollAdmin/Ajax/DepartamentSearch",
            method: "Get",
            data: { name: name },
            success: function (res) {
                if (res.status == 200) {
                    $("#departamentNotFoundError").html("");
                    addNewDepartament(res.data);
                }
                else if (res.status == 400) {
                    $("#departamentNotFoundError").html("");
                    var span = document.createElement('span');
                    span.textContent = "Belə Departament yoxdur !!!";
                    $("#departamentNotFoundError").append(span);
                    addNewDepartament(res.data);
                }
            }
        });
    })

    //@*-------- shop create ---------------------------------*@

    $(document).on("click", ".shopCreate", function (e) {
        $("#shopCreate").modal("show");
    });

    $(document).on("click", ".shopCreateSave", function (e) {
        let shopName = $("#shopNameCreate").val();
        let selectedCompanyId = $("#selectCompany").val();
        let image = $
        if (shopName) {
            var ajax = ({
                url: "/PayrollAdmin/Shop/Create",
                data: { name: shopName, companyId: selectedCompanyId, },
                method: "Post",
                success: function (respons) {
                    if (respons.message == 200) {
                        alert("Əməliyyat uğurludur: Mağaza əlavə edildi...")
                    }
                }
            })
        }

        var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
        if (antiForgeryToken) {
            ajax.headers = {};
            ajax.headers["X-XSRF-Token"] = antiForgeryToken;
        };

        $.ajax(ajax);
    });

    //@*-------- shop dayProfit ---------------------------------*@

    let shopProfitShopId;

    $(document).on("click", ".dayProfit", function () {
        shopProfitShopId = $(this).data("id");
        $("#dayProfit").modal("show");
    });

    $(document).on("click", ".shopModalProfitCreate", function () {
        let shopProfit = $("#shopProfit").val();
        let shopProfitDate = $("#shopProfitDate").val();
        var ajax = ({
            url: "/PayrollAdmin/Ajax/ShopProfitWrite",
            data: { shopId: shopProfitShopId, date: shopProfitDate, profit: shopProfit },
            method: "Post",
            success: function (respons) {
                if (respons.message == 200) {
                    alert("Əməliyyat uğurludur: Mağazanın bu günki qazancı əlavə edildi...")
                }
            }
        })

        var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
        if (antiForgeryToken) {
            ajax.headers = {};
            ajax.headers["X-XSRF-Token"] = antiForgeryToken;
        };

        $.ajax(ajax);
    });

    //@*-------- shop delete ---------------------------------*@

    let shopId, deletedShop;

    $(document).on("click", ".shopDelete", function () {
        shopId = $(this).data("id");
        console.log(shopId);
        deletedShop = $(this).parents();
        if (shopId) {
            $.ajax({
                url: "/PayrollAdmin/Shop/Delete",
                method: "Get",
                data: { id: shopId },
                success: function (respons) {
                    if (respons.message == 200) {
                        $("#shopName").text(respons.shopDb.name);
                        $('#shopDelete').modal('show')
                    }
                },
                error: function (xhr, status, error) {
                    console.log(xhr.status);
                    console.log(status);
                    console.log(error);
                }
            });
        }
    });

    $(document).on("click", ".shopDeleteSave", function (e) {
        e.preventDefault();
        var ajax = ({
            url: "/PayrollAdmin/Shop/DeletePost",
            method: "POST",
            async: true,
            data: { id: shopId },
            success: function (respons) {
                if (respons.message == 200) {
                    $(deletedShop).parents("div.col-xl-3").remove();
                }
            }
        });

        var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
        if (antiForgeryToken) {
            ajax.headers = {};
            ajax.headers["X-XSRF-Token"] = antiForgeryToken;
        };

        $.ajax(ajax);
    });

    //@-------- shop edit ------------------------------------*@

    let shopEditId;

    $(document).on("click", ".shopEdit", function (e) {
        e.preventDefault();
        shopEditId = $(this).data("id");
        $.ajax({
            url: "/PayrollAdmin/Shop/Edit",
            data: { id: shopEditId },
            type: "Get",
            success: function (respons) {
                if (respons.message == 200) {
                    console.log("hgf");
                    $("#shopNameUpdate").val(respons.shopDb.name);
                    $('#shopEdit').modal('show')
                }
            }
        });
    });

    $(document).on("click", ".shopEditSave", function (e) {
        e.preventDefault();
        let name = $("#shopNameUpdate").val();
        if (name) {
            var ajax = {
                url: "/PayrollAdmin/Shop/EditPost",
                data: { id: shopEditId, name: name },
                type: "Post",
                success: function (respons) {
                    if (respons.message == 200) {
                        alert("Əməliyyat uğurludur: Mağazanın adı redaktə olundu...")
                    }
                }
            }

            var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
            if (antiForgeryToken) {
                ajax.headers = {};
                ajax.headers["X-XSRF-Token"] = antiForgeryToken;
            };
            $.ajax(ajax);
        }
    });

    //@* ------ shop search ----------------------------------*@

    function addNewShop(datas) {
        var maindiv = $("#divShop").clone();
        $("div.divShopClone").html(maindiv);
        for (var data of datas) {
            var div = $("#divShop").clone();
            div.removeClass("d-none");
            div.removeAttr('id');
            div.find("h3").text(data.name);
            div.find("ul li:nth-child(1) button").attr("data-id", data.id);
            div.find("ul li:nth-child(2) button").attr("data-id", data.id);
            $("div.divShopClone").append(div);
        }
    }

    $(document).on("click", ".searchShop", function (e) {
        e.preventDefault();
        var name = $("#shopAjaxName").val();
        $.ajax({
            url: "/PayrollAdmin/Ajax/ShopSearch",
            method: "Get",
            data: { name: name },
            success: function (res) {
                if (res.status == 200) {
                    $("#shopNotFoundError").html("");
                    addNewShop(res.data);
                }
                else if (res.status == 400) {
                    $("#shopNotFoundError").html("");
                    var span = document.createElement('span');
                    span.textContent = "Belə Mağaza yoxdur !!!";
                    $("#shopNotFoundError").append(span);
                    addNewShop(res.data);
                }
            }
        });
    })

    //--------- shop - bonus - create ------------------------------------

    let deletedshopBonus = [];
    $('.checkbox').click(function () {
        deletedshopBonus.push(this);
    });

    $(document).on("click", ".shopProfitCreate", function (e) {
        e.preventDefault();
        let bonusShops = []; let mountProfit;
        $(".shopProfit tbody tr").each(function (index, value) {
            if ($(value).find("[class='checkbox']").is(":checked")) {
                var bonusShop = {
                    shopid: $(value).find("[class='checkbox']:checked").data("id"),
                    MinAmount: $(this).find("td:nth-child(3)").text(),
                    PromotionAmount: $(this).find("td:nth-child(7)").text(),
                };

                bonusShops.push(bonusShop);
                mountProfit = $(this).find("td:nth-child(4").text();
            }
        });

        var ajax = ({
            url: "/PayrollAdmin/Ajax/ShopBonus",
            data: { shopBonus: bonusShops, mountProfit: mountProfit},
            type: "Post",
            success: function (respons) {
                if (respons.message == 200) {
                    $(deletedshopBonus).parents("tr").remove();
                    alert("secdiyiniz magazalardaki iscilere bonus ugurla elave olundu...")
                }
                else if (respons.message == 400) {
                    alert("hec bir mağazanı secmemisiz...");
                }
            }
        });

        var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
        if (antiForgeryToken) {
            ajax.headers = {};
            ajax.headers["X-XSRF-Token"] = antiForgeryToken;
        };

        $.ajax(ajax);

    });

    //@*-------- position create -----------------------------*@

    let positionCreateName;
    $(document).on("click", ".positionCreate", function (e) {
        $("#positionCreate").modal("show");
    });

    $(document).on("click", ".positionCreateSave", function (e) {
        positionCreateName = $(".positionCreateName").val();
        let positionDepartmantId = $("#Departmant_Id").val();
        if (positionDepartmantId && positionCreateName) {
            var ajax = ({
                url: "/PayrollAdmin/Position/Create",
                data: { id: positionDepartmantId, name: positionCreateName },
                method: "Post",
                success: function (respons) {
                    if (respons.message == 200) {
                        alert("Əməliyyat uğurludur: Vəzifənin adı databazaya əlavə edildi...")
                    }
                }
            })
        }

        var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
        if (antiForgeryToken) {
            ajax.headers = {};
            ajax.headers["X-XSRF-Token"] = antiForgeryToken;
        };

        $.ajax(ajax);
    });

    //@*----------- position delete ------------------------------*@

    let positionId, deletedPosition;
    $(document).on("click", ".deletePosition", function () {
        positionId = $(this).data("id");
        deletedPosition = $(this).parents();
        console.log(deletedPosition);
        console.log(deletedPosition)
        if (positionId) {
            $.ajax({
                url: "/PayrollAdmin/Position/Delete",
                method: "Get",
                data: { id: positionId },
                success: function (respons) {
                    if (respons.message == 200) {
                        $("#positionDeleteName").text(respons.positionDb.name);
                        $('#delete').modal('show')
                    }
                },
                error: function (xhr, status, error) {
                    console.log(xhr.status);
                    console.log(status);
                    console.log(error);
                }
            });
        }
    });

    $(document).on("click", ".positionDeleteSave", function (e) {
        e.preventDefault();
        var ajax = ({
            url: "/PayrollAdmin/Position/DeletePost",
            method: "POST",
            async: true,
            data: { id: positionId },
            success: function (respons) {
                if (respons.message == 200) {
                    $(deletedPosition).parents("tr.shop").remove();
                }
            }
        });

        var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
        if (antiForgeryToken) {
            ajax.headers = {};
            ajax.headers["X-XSRF-Token"] = antiForgeryToken;
        };

        $.ajax(ajax);
    });

    //@*----------- position changed recruitment search -----*@

    function addNewChangedPosition(datas) {
        var maintr = $("#trChangedPosition").clone();
        $("table.positionChanged tbody").html(maintr);
        for (var data of datas) {
            var tr = $("#trChangedPosition").clone();
            tr.removeClass("d-none");
            tr.removeAttr('id');
            tr.find("img").attr("src", "/img/" + data.image);
            tr.find("td:nth-child(2)").text(data.firstname);
            tr.find("td:nth-child(3)").text(data.lastname);
            tr.find("td:nth-child(4)").text(data.phone);
            tr.find("td:nth-child(5)").text(data.email);
            tr.find("td:nth-child(6)").text(data.name);
            tr.find("td:nth-child(7)").text(data.whenStarted);
            var td = $(tr.find("td:nth-child(8)"));
            td.find("a:nth-child(1)").attr({ href: "/PayrollAdmin/Position/PositionChanged/" + data.id })
            $("table.positionChanged tbody").append(tr);
        }
    }

    $(document).on("click", ".searchChangedPosition", function (e) {
        e.preventDefault();
        var name = $("#positionChangedName").val();
        $.ajax({
            url: "/PayrollAdmin/Ajax/PositionChangedSearch",
            method: "Get",
            data: { name: name },
            success: function (res) {
                if (res.status == 200) {
                    addNewChangedPosition(res.data);
                    $("#positionChangedNotFoundError").html("");

                }
                else if (res.status == 400) {
                    $("#positionChangedNotFoundError").html("");
                    var span = document.createElement('span');
                    span.textContent = "Belə İşçi yoxdur !!!";
                    $("#positionChangedNotFoundError").append(span);
                    addNewChangedPosition(res.data);

                }
            }
        });
    })

    //@*----------- position edit ------------------------------*@

    let positionEditId;
    $(document).on("click", ".positionEdit", function (e) {
        positionEditId = $(this).data("id");
        console.log(positionEditId);
        $.ajax({
            url: "/PayrollAdmin/Position/Edit",
            data: { id: positionEditId },
            type: "Get",
            success: function (respons) {
                if (respons.message == 200) {
                    $("#positionNameUpdate").val(respons.positionDb.name);
                    $('#positionEdit').modal('show')
                }
            }
        });
    });

    $(document).on("click", ".positionEditSave", function (e) {
        e.preventDefault();
        let name = $("#positionNameUpdate").val();
        if (name) {
            var ajax = {
                url: "/PayrollAdmin/Position/EditPost",
                data: { id: positionEditId, name: name },
                type: "Post",
                success: function (respons) {
                    if (respons.message == 200) {
                        alert("Əməliyyat uğurludur: Vəzifənin adı redaktə edildi...")
                    }
                }
            }

            var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
            if (antiForgeryToken) {
                ajax.headers = {};
                ajax.headers["X-XSRF-Token"] = antiForgeryToken;
            };
            $.ajax(ajax);
        }
    });

    //@* --------- position search -----------------------------*@

    function addNewPosition(datas) {
        var maintr = $("#trPosition").clone();
        $("table.positions tbody").html(maintr);
        for (var data of datas) {
            var tr = $("#trPosition").clone();
            tr.removeClass("d-none");
            tr.removeAttr('id');
            tr.find("td:nth-child(1)").text(data.name);
            tr.find("td:nth-child(2)").text(data.departamentName);
            tr.find("td:nth-child(3) button").attr("data-id", data.id);
            $("table.positions tbody").append(tr);
        }
    }

    $(document).on("click", ".searchPosition", function (e) {
        e.preventDefault();
        var name = $("#positionName").val();
        $.ajax({
            url: "/PayrollAdmin/Ajax/PositionSearch",
            method: "Get",
            data: { name: name },
            success: function (res) {
                if (res.status == 200) {
                    $("#positionNotFoundError").html("");
                    addNewPosition(res.data);
                    console.log(res.data);
                }
                else if (res.status == 400) {
                    $("#positionNotFoundError").html("");
                    var span = document.createElement('span');
                    span.textContent = "Belə Vəzifə yoxdur !!!";
                    $("#positionNotFoundError").append(span);
                    addNewPosition(res.data);
                    console.log(res.data);

                }
            }
        });
    })

    //@* --------- employee search -----------------------------*@

    function addNewEmployee(datas) {
        var maintr = $("#trEmployee").clone();
        $("table.trEmployeeClone tbody").html(maintr);
        for (var data of datas) {
            var tr = $("#trEmployee").clone();
            tr.removeClass("d-none");
            tr.removeAttr('id');
            tr.find("img").attr("src", "/img/" + data.image);
            tr.find("td:nth-child(2)").text(data.firstname);
            tr.find("td:nth-child(3)").text(data.lastname);
            tr.find("td:nth-child(4)").text(data.fathername);
            tr.find("td:nth-child(5)").text(data.phone);
            tr.find("td:nth-child(6)").text(data.email);
            var td = $(tr.find("td:nth-child(7)"));
            td.find("a:nth-child(1)").attr({ href: "/PayrollAdmin/Employee/Edit/" + data.id })
            td.find("a:nth-child(2)").attr({ href: "/PayrollAdmin/Employee/Details/" + data.id })
            td.find("a:nth-child(3)").attr("data-id", data.id);
            td.find("a:nth-child(4)").attr({ href: "/PayrollAdmin/Employee/Recruitment/" + data.id })
            $("table.trEmployeeClone tbody").append(tr);
        }
    }

    $(document).on("click", ".searchEmployee", function (e) {
        e.preventDefault();
        var name = $("#employeeName").val();
        $.ajax({
            url: "/PayrollAdmin/Ajax/EmployeeSearch",
            method: "Get",
            data: { name: name },
            success: function (res) {
                if (res.status == 200) {
                    $("#employeeNotFoundError").html("");
                    addNewEmployee(res.data);
                }
                else if (res.status == 400) {
                    $("#employeeNotFoundError").html("");
                    var span = document.createElement('span');
                    span.textContent = "Belə Namizəd yoxdur !!!";
                    $("#employeeNotFoundError").append(span);
                    addNewEmployee(res.data);
                }
            }
        });
    })

    //@* --------- employee delete ---------------------------------*@

    let employeeId, deletedEmployee;
    $(document).on("click", ".deleteEmployee", function (e) {
        e.preventDefault();
        employeeId = $(this).data("id");
        deletedEmployee = $(this).parents();
        $.ajax({
            url: "/PayrollAdmin/Employee/Delete",
            method: "Get",
            data: { id: employeeId },
            success: function (respons) {
                if (respons.message == 200) {
                    $("#employeeNameDelete").text(respons.employeeDb.firstname);
                    $('#delete').modal('show');
                }
            }
        });

    });

    $(document).on("click", ".employeeDeleteSave", function (e) {
        e.preventDefault();
        var ajax = ({
            url: "/PayrollAdmin/Employee/DeletePost",
            method: "POST",
            async: true,
            data: { id: employeeId },
            success: function (respons) {
                if (respons.message == 200) {
                    $(deletedEmployee).parents("tr").remove();
                }
            }
        });

        var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
        if (antiForgeryToken) {
            ajax.headers = {};
            ajax.headers["X-XSRF-Token"] = antiForgeryToken;
        };

        $.ajax(ajax);
    });

    //@* --------- recruitment search-------------------------------*@

    function addNewRecruitment(datas) {
        var maintr = $("#trRecruitment").clone();
        $("table.recruitment tbody").html(maintr);
        for (var data of datas) {
            var tr = $("#trRecruitment").clone();
            tr.removeClass("d-none");
            tr.removeAttr('id');
            tr.find("img").attr("src", "/img/" + data.image);
            tr.find("td:nth-child(2)").text(data.firstname);
            tr.find("td:nth-child(3)").text(data.lastname);
            tr.find("td:nth-child(4)").text(data.role);
            tr.find("td:nth-child(5)").text(data.phone);
            tr.find("td:nth-child(6)").text(data.email);
            var td = $(tr.find("td:nth-child(7)"));
            td.find("a:nth-child(1)").attr({ href: "/PayrollAdmin/Recruitment/Edit/" + data.id })
            td.find("a:nth-child(2)").attr({ href: "/PayrollAdmin/Recruitment/Details/" + data.id })
            td.find("a:nth-child(3)").attr("data-id", data.id);
            td.find("a:nth-child(4)").attr({ href: "/PayrollAdmin/Recruitment/AddRole/" + data.id })
            $("table.recruitment tbody").append(tr);
        }
    }

    $(document).on("click", ".searchRecruitment", function (e) {
        e.preventDefault();
        var name = $("#recruitmentName").val();
        $.ajax({
            url: "/PayrollAdmin/Ajax/RecruitmentSearch",
            method: "Get",
            data: { name: name },
            success: function (res) {
                console.log(res.data);
                if (res.status == 200) {
                    $("#recruitmentNameError").html("");
                    addNewRecruitment(res.data);
                }
                else if (res.status == 400) {
                    $("#recruitmentNameError").html("");
                    var span = document.createElement('span');
                    span.textContent = "Belə İşçi yoxdur!!!";
                    $("#recruitmentNameError").append(span);
                    addNewRecruitment(res.data);
                }
            }
        });
    })

    //@* --------- recruitment delete ------------------------------*@

    let recruitmentDeletedId, deletedRecruitment;
    $(document).on("click", ".deleteRecruitment", function (e) {
        e.preventDefault();
        recruitmentDeletedId = $(this).data("id");
        deletedRecruitment = $(this).parents();
        $.ajax({
            url: "/PayrollAdmin/Recruitment/Delete",
            method: "Get",
            data: { id: recruitmentDeletedId },
            success: function (respons) {
                if (respons.message == 200) {
                    $("#recruitmentNameDelete").text(respons.recruitmentDb.firstname);
                    $('#deleteRecruitment').modal('show');
                }
            }
        });

    });

    $(document).on("click", ".deleteRecruitmentSave", function (e) {
        e.preventDefault();
        var ajax = ({
            url: "/PayrollAdmin/Recruitment/DeletePost",
            method: "POST",
            async: true,
            data: { id: recruitmentDeletedId },
            success: function (respons) {
                if (respons.message == 200) {
                    $(deletedRecruitment).parents("tr").remove();
                }
                else if (respons.message == 400) {
                    alert("ugursuzluq...");
                }
            }
        });

        var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
        if (antiForgeryToken) {
            ajax.headers = {};
            ajax.headers["X-XSRF-Token"] = antiForgeryToken;
        };

        $.ajax(ajax);
    });

    //@* --------- selectListItem combobox changed ------------------*@

    $("#companyId").on("change", function () {
        var companyId = $("#companyId").val();
        $.ajax({
            url: "/PayrollAdmin/Ajax/Departament",
            data: { id: companyId },
            type: "Post",
            success: function (respons) {
                if (respons.message == 200) {
                    $("#departamentId").html("");
                    $("#departamentError").html("");
                    for (var item of respons.departament) {
                        var option = document.createElement("option");
                        option.text = item.name;
                        option.value = item.id;
                        $("#departamentId").append(option);
                    }
                }
                else if (respons.message == 400) {
                    $("#departamentId").html("");
                    $("#departamentError").html("");
                    var span = document.createElement('span');
                    span.textContent = "Bu şirkətə aid departament yoxdur";
                    $("#departamentError").append(span);
                }
            }

        });

        $.ajax({
            url: "/PayrollAdmin/Ajax/Shop",
            data: { id: companyId },
            type: "Post",
            dateType: "Json",
            success: function (respons) {
                if (respons.message == 200) {
                    $("#shopId").html("");
                    $('#shopError').html("");
                    for (var item of respons.shop) {
                        var option = document.createElement('option');
                        option.text = item.name;
                        option.value = item.id;
                        $("#shopId").append(option);
                    }
                }
                else if (respons.message == 400) {
                    $("#shopId").html("");
                    $('#shopError').html("");
                    var span = document.createElement('span');
                    span.textContent = "Bu şirkətə aid mağaza yoxdur";
                    $("#shopError").append(span);
                }
            }
        });

    });

    $("#departamentId").change(function (e) {
        var departamentId = $("#departamentId").val();
        $.ajax({
            url: "/PayrollAdmin/Ajax/Position",
            data: { id: departamentId },
            type: "Post",
            success: function (respons) {
                if (respons.message == 200) {
                    $("#positionId").html("");
                    $('#positionError').html("");
                    for (var item of respons.shop) {
                        var option = document.createElement('option');
                        option.text = item.name;
                        option.value = item.id;
                        $("#positionId").append(option);
                    }
                }
                else if (respons.message == 400) {
                    $("#positionId").html("");
                    $('#positionError').html("");
                    var span = document.createElement('span');
                    span.textContent = "Bu departamentə aid vəzifə yoxdur";
                    $("#positionError").append(span);
                }
            }
        });
    })

    $("#positionId").change(function (e) {
        var positionId = $("#positionId").val();
        $.ajax({
            url: "/PayrollAdmin/Ajax/Salary",
            data: { id: positionId },
            type: "Post",
            success: function (respons) {
                if (respons.message == 200) {
                    $("#salary").html("");
                    $('#salaryError').html("");
                    $("#salary").val(respons.salary[0].salaryAmount);
                    console.log(respons.salary[0].salaryAmount);
                }
                else if (respons.message == 400) {
                    $("#salary").html("");
                    $('#salaryError').html("");
                    var span = document.createElement('span');
                    span.textContent = "Bu vəzifəyə aid maaş yoxdur";
                    $("#salaryError").append(span);
                }
            }
        });
    })

    //@* --------- absence createAjax -------------------------------*@

    let deleted = [];
    $('.checkbox').click(function () {
        deleted.push(this);
    });

    $(document).on("click", ".createSaveAbsence", function (e) {
        e.preventDefault();
        let continuities = [];
        $(".continuity tbody tr").each(function (index, value) {
            if ($(value).find("[class='checkbox']").is(":checked")) {
                var continuity = {
                    recruitmentid: $(value).find("[class='checkbox']:checked").data("id"),
                    reason: $(value).find("input[name='Continuity.Reason']").val(),
                    permissiontype: $(value).find("select[name='Continuity.PermissionType']").val(),
                    date: $("#dateContinuity").val()
                };
                continuities.push(continuity);
            }
        });

        var ajax = ({
            url: "/PayrollAdmin/Absence/CreateAjax",
            data: { continuities: continuities },
            type: "POST",
            success: function (respons) {
                if (respons.message == 200) {
                    alert("Seçdiyiniz işçiyə və ya işçilərə qayıb əlavə olundu.")
                    $(deleted).parents("tr").remove();
                    $(deletedAjax).parents("tr").remove();
                }
                else if (respons.message == 400) {
                    alert("Zəhmət olmasa qayıb yazacağınız işçini və ya işçiləri seçin.")
                }
                else if (respons.message == 300) {
                    alert("Zəhmət olmasa səbəbi daxil edin.")
                }
            }
        });

        var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
        if (antiForgeryToken) {
            ajax.headers = {};
            ajax.headers["X-XSRF-Token"] = antiForgeryToken;
        };

        $.ajax(ajax);

    });

    //@* --------- absence search -----------------------------------------------------------------------------------------------------*@

    let deletedAjax = [];
    function absenceWrite(datas) {
        var maintr = $("#trContinuity").clone();
        $("table.continuity tbody").html(maintr);
        for (var data of datas) {
            var tr = $("#trContinuity").clone();
            tr.removeClass("d-none");
            tr.removeAttr('id');
            tr.find("img").attr("src", "/img/" + data.image);
            tr.find("td:nth-child(2)").text(data.firstname);
            tr.find("td:nth-child(3)").text(data.lastname);
            tr.find("td:nth-child(4)").text(data.permissionType);
            tr.find("td:nth-child(5)").val(data.reason);
            tr.find("td:nth-child(6) input").attr("data-id", data.id);
            $("table.continuity tbody").append(tr);
            $('.checkbox').click(function () {
                deletedAjax.push(this);
            });
        }
    }

    $(document).on("click", ".searchContinuity", function (e) {
        e.preventDefault();
        var date = $("#dateContinuity").val();
        $.ajax({
            url: "/PayrollAdmin/Absence/SearchList",
            method: "Get",
            data: { date: date },
            success: function (res) {
                if (res.status == 200) {
                    $("#absenceSuccess").html("");
                    $("#absenceUnsuccessful").html("");
                    var span = document.createElement('span');
                    span.textContent = "Seçilmiş tarixdə aşağıdakı işçilərin qayıbı yoxdur...";
                    $("#absenceSuccess").append(span);
                    absenceWrite(res.data);
                }
                else if (res.status == 400) {
                    $("#absenceUnsuccessful").html("");
                    $("#absenceSuccess").html("");
                    var span = document.createElement('span');
                    span.textContent = "Seçilmiş tarixə qayıb yazmaq olmaz...";
                    $("#absenceUnsuccessful").append(span);
                    absenceWrite(res.data);

                }
            }
        });
    })

    //@* --------- bonus add -----------------------------------------------------------------------------------------------------*@

    let recruitmentId, bonusRecruitmentAdded;
    $(document).on("click", ".bonus", function (e) {
        $('#myModal').modal('show')
        recruitmentId = $(this).data("id");
        bonusRecruitmentAdded = $(this).parents();
    });

    $(document).on("click", ".save", function (e) {
        e.preventDefault();
        let amount = $("#amount").val();
        let reason = $("#reason").val();
        if (recruitmentId && amount && reason) {
            $.ajax({
                url: "/PayrollAdmin/Bonus/Create",
                method: "POST",
                data: { id: recruitmentId, amount: amount, reason: reason },
                success: function (res) {
                    if (res.status == 200) {
                        console.log("bonussss")
                        $("#amount").val("");
                        $("#reason").val("");
                        alert("Elave olundu....")
                        $(bonusRecruitmentAdded).parents("tr").remove();
                    }
                }
            });
        }
    });

    //@* --------- bonus edit -----------------------------------------------------------------------------------------------------*@

    let bonusEditRecruitmentId;
    $(document).on("click", ".edit", function (e) {
        bonusEditRecruitmentId = $(this).data("id");
        $.ajax({
            url: "/PayrollAdmin/Bonus/Edit",
            data: { id: bonusEditRecruitmentId },
            type: "Get",
            success: function (respons) {
                if (respons.message == 200) {
                    $("#amountEdit").val(respons.bonusDb.amount);
                    $("#reasonEdit").val(respons.bonusDb.reason);
                    jQuery.data($(".saveEdit")[0], "bonuseditid", respons.bonusDb.id)
                }
                if (respons.message == 400) {
                    $("#amountEdit").val("");
                    $("#reasonEdit").val("");
                }
            }
        });

        $('#myEdit').modal('show')
    });

    $(document).on("click", ".saveEdit", function (e) {
        e.preventDefault();
        let amount = $("#amountEdit").val();
        let reason = $("#reasonEdit").val();
        if (bonusEditRecruitmentId && amount && reason) {
            var ajax = ({
                url: "/PayrollAdmin/Bonus/Edit",
                method: "POST",
                data: { id: bonusEditRecruitmentId, amount: amount, reason: reason },
                success: function (respons) {
                    if (respons.message == 200) {
                        $("#amount").val("");
                        $("#reason").val("");
                        alert("Uğurlu olduuu...")
                    }
                }
            });

            var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
            if (antiForgeryToken) {
                ajax.headers = {};
                ajax.headers["X-XSRF-Token"] = antiForgeryToken;
            };
            $.ajax(ajax);
        }
    });

    //@* --------- bonusList search -----------------------------------------------------------------------------------------------------*@

    function addNewRecruitmentBonusList(datas) {
        var maintr = $("#bonusList").clone();
        $("table.bonusList tbody").html(maintr);
        for (var data of datas) {
            var tr = $("#bonusList").clone();
            tr.removeClass("d-none");
            tr.removeAttr('id');
            tr.find("img").attr("src", "/img/" + data.image);
            tr.find("td:nth-child(2)").text(data.firstname);
            tr.find("td:nth-child(3)").text(data.lastname);
            tr.find("td:nth-child(4)").text(data.email);
            tr.find("td:nth-child(5)").text(data.name);
            tr.find("td:nth-child(6)").text(data.positionName);
            tr.find("td:nth-child(7)").text(data.amount);
            tr.find("td:nth-child(8)").text(data.reason);
            tr.find("td:nth-child(9) button").attr("data-id", data.recruitmentId);
            $("table.bonusList tbody").append(tr);
        }
    }

    $(document).on("click", ".searchBonusList", function (e) {
        e.preventDefault();
        var name = $("#recruitmentBonusList").val();
        $.ajax({
            url: "/PayrollAdmin/Ajax/BonusListSearch",
            method: "Get",
            data: { name: name },
            success: function (res) {
                console.log(res.data);
                if (res.status == 200) {
                    $("#recruitmentBonusListError").html("");
                    addNewRecruitmentBonusList(res.data);
                }
                else if (res.status == 400) {
                    $("#recruitmentBonusListError").html("");
                    var span = document.createElement('span');
                    span.textContent = "Axtarışda belə işçi tapılmadı :(((";
                    $("#recruitmentBonusListError").append(span);
                    addNewRecruitmentBonusList(res.data);
                }
            }
        });
    })

    //@* --------- bonusCrate search -----------------------------------------------------------------------------------------------------*@

    function addNewRecruitmentBonusCreate(datas) {
        var maintr = $("#bonusCreateClone").clone();
        $("table.bonusCreate tbody").html(maintr);
        for (var data of datas) {
            var tr = $("#bonusCreateClone").clone();
            tr.removeClass("d-none");
            tr.removeAttr('id');
            tr.find("img").attr("src", "/img/" + data.image);
            tr.find("td:nth-child(2)").text(data.firstname);
            tr.find("td:nth-child(3)").text(data.lastname);
            tr.find("td:nth-child(4)").text(data.fathername);
            tr.find("td:nth-child(5)").text(data.email);
            tr.find("td:nth-child(6)").text(data.name);
            tr.find("td:nth-child(7)").text(data.positionName);
            tr.find("td:nth-child(8) button").attr("data-id", data.id);
            $("table.bonusCreate tbody").append(tr);
        }
    }

    $(document).on("click", ".searchBonusCreate", function (e) {
        e.preventDefault();
        var name = $("#recruitmentBonusCreate").val();
        $.ajax({
            url: "/PayrollAdmin/Ajax/BonusCreateSearch",
            method: "Get",
            data: { name: name },
            success: function (res) {
                console.log(res.data);
                if (res.status == 200) {
                    $("#recruitmentBonusCreateError").html("");
                    addNewRecruitmentBonusCreate(res.data);
                }
                else if (res.status == 400) {
                    $("#recruitmentBonusCreateError").html("");
                    var span = document.createElement('span');
                    span.textContent = "Axtarışda belə işçi tapılmadı :(((";
                    $("#recruitmentBonusCreateError").append(span);
                    addNewRecruitmentBonusCreate(res.data);
                }
            }
        });
    })

    //@* --------- penaltyList search -----------------------------------------------------------------------------------------------------*@

    function addNewRecruitmentPenaltyList(datas) {
        var maintr = $("#penaltyList").clone();
        $("table.penaltyList tbody").html(maintr);
        for (var data of datas) {
            var tr = $("#penaltyList").clone();
            tr.removeClass("d-none");
            tr.removeAttr('id');
            tr.find("img").attr("src", "/img/" + data.image);
            tr.find("td:nth-child(2)").text(data.firstname);
            tr.find("td:nth-child(3)").text(data.lastname);
            tr.find("td:nth-child(4)").text(data.email);
            tr.find("td:nth-child(5)").text(data.name);
            tr.find("td:nth-child(6)").text(data.positionName);
            tr.find("td:nth-child(7)").text(data.amount);
            tr.find("td:nth-child(8)").text(data.reason);
            tr.find("td:nth-child(9) button").attr("data-id", data.recruitmentId);
            $("table.penaltyList tbody").append(tr);
        }
    }

    $(document).on("click", ".searchPenaltyList", function (e) {
        e.preventDefault();
        var name = $("#recruitmentPenaltyList").val();
        $.ajax({
            url: "/PayrollAdmin/Ajax/PenaltyListSearch",
            method: "Get",
            data: { name: name },
            success: function (res) {
                console.log(res.data);
                if (res.status == 200) {
                    $("#recruitmentPenaltyListError").html("");
                    addNewRecruitmentPenaltyList(res.data);
                }
                else if (res.status == 400) {
                    $("#recruitmentPenaltyListError").html("");
                    var span = document.createElement('span');
                    span.textContent = "Axtarışda belə işçi tapılmadı :(((";
                    $("#recruitmentPenaltyListError").append(span);
                    addNewRecruitmentPenaltyList(res.data);
                    console.log(res.data);
                }
            }
        });
    })

    //@* --------- penaltyCreate search -----------------------------------------------------------------------------------------------------*@

    function addNewRecruitmentPenaltyCreate(datas) {
        var maintr = $("#penaltyCreateClone").clone();
        $("table.penaltyCreateClone tbody").html(maintr);
        for (var data of datas) {
            var tr = $("#penaltyCreateClone").clone();
            tr.removeClass("d-none");
            tr.removeAttr('id');
            tr.find("img").attr("src", "/img/" + data.image);
            tr.find("td:nth-child(2)").text(data.firstname);
            tr.find("td:nth-child(3)").text(data.lastname);
            tr.find("td:nth-child(4)").text(data.fathername);
            tr.find("td:nth-child(5)").text(data.email);
            tr.find("td:nth-child(6)").text(data.name);
            tr.find("td:nth-child(7)").text(data.positionName);
            tr.find("td:nth-child(8) button").attr("data-id", data.id);
            $("table.penaltyCreateClone tbody").append(tr);
        }
    }

    $(document).on("click", ".searchPenaltyCreate", function (e) {
        e.preventDefault();
        var name = $("#recruitmentPenaltyCreate").val();
        $.ajax({
            url: "/PayrollAdmin/Ajax/PenaltyCreateSearch",
            method: "Get",
            data: { name: name },
            success: function (res) {
                console.log(res.data);
                if (res.status == 200) {
                    $("#recruitmentPenaltyCreateError").html("");
                    addNewRecruitmentPenaltyCreate(res.data);
                }
                else if (res.status == 400) {
                    $("#recruitmentPenaltyCreateError").html("");
                    var span = document.createElement('span');
                    span.textContent = "Axtarışda belə işçi tapılmadı :(((";
                    $("#recruitmentPenaltyCreateError").append(span);
                    addNewRecruitmentPenaltyCreate(res.data);
                    console.log(res.data);
                }
            }
        });
    })

    //@* --------- penalty add -----------------------------------------------------------------------------------------------------*@

    let penaltyId, penaltyDeleted;
    $(document).on("click", ".penalty", function (e) {
        $('#myModal').modal('show')
        penaltyId = $(this).data("id");
        penaltyDeleted = $(this).parents();
    });

    $(document).on("click", ".penaltyCreate", function (e) {
        e.preventDefault();
        let amount = $("#penaltyAmountCreate").val();
        let reason = $("#penaltyAmountReason").val();
        if (penaltyId && amount && reason) {
            $.ajax({
                url: "/PayrollAdmin/Penalty/Create",
                method: "POST",
                data: { id: penaltyId, amount: amount, reason: reason },
                success: function (res) {
                    console.log(res);
                    if (res.status === 200) {
                        console.log("penaltyyyy")
                        $("#penaltyAmountCreate").val("");
                        $("#penaltyAmountReason").val("");
                        //alert("Elave olundu....")
                        //$(penaltyDeleted).parents().remove();
                    }
                }
            });
        }
    });

    //@* --------- penalty edit -----------------------------------------------------------------------------------------------------*@

    let penaltyEditRecruitmentId;
    $(document).on("click", ".penaltyEdit", function (e) {
        penaltyEditRecruitmentId = $(this).data("id");
        $.ajax({
            url: "/PayrollAdmin/Penalty/Edit",
            data: { id: penaltyEditRecruitmentId },
            type: "Get",
            success: function (respons) {
                if (respons.message == 200) {
                    $("#penaltyAmount").val(respons.bonusDb.amount);
                    $("#penaltyReason").val(respons.bonusDb.reason);
                    jQuery.data($(".penaltySave")[0], "bonuseditid", respons.bonusDb.id)
                }
                if (respons.message == 400) {
                    $("#penaltyAmount").val("");
                    $("#penaltyReason").val("");
                }
            }
        });

        $('#myEdit').modal('show')
    });

    $(document).on("click", ".penaltySave", function (e) {
        e.preventDefault();
        let amount = $("#penaltyAmount").val();
        let reason = $("#penaltyReason").val();
        if (penaltyEditRecruitmentId && amount && reason) {
            var ajax = ({
                url: "/PayrollAdmin/Penalty/Edit",
                method: "POST",
                data: { id: penaltyEditRecruitmentId, amount: amount, reason: reason },
                success: function (respons) {
                    if (respons.message == 200) {
                        $("#penaltyAmount").val("");
                        $("#penaltyReason").val("");
                        alert("Uğurlu olduuu...")
                    }
                }
            });

            var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
            if (antiForgeryToken) {
                ajax.headers = {};
                ajax.headers["X-XSRF-Token"] = antiForgeryToken;
            };
            $.ajax(ajax);
        }
    });

    //@* --------- payrollList search -----------------------------------------------------------------------------------------------------*@

    function addNewPayrollList(datas) {
        var maintr = $("#trPayroll").clone();
        $("table.payrolls tbody").html(maintr);
        for (var data of datas) {
            console.log(data)
            var tr = $("#trPayroll").clone();
            tr.removeClass("d-none");
            tr.removeAttr('id');
            tr.find("img").attr("src", "/img/" + data.image);
            tr.find("td:nth-child(2)").text(data.firstname);
            tr.find("td:nth-child(3)").text(data.lastname);
            tr.find("td:nth-child(4)").text(data.name);
            tr.find("td:nth-child(5)").text(data.positionName);
            tr.find("td:nth-child(6)").text(data.shopname);
            tr.find("td:nth-child(7)").text(data.totalSalary);
            $("table.payrolls tbody").append(tr);
        }
    }

    $(document).on("click", ".payrollSearch", function (e) {
        e.preventDefault();
        var name = $("#payrollName").val();
        var date = $("#payrollDate").val();
        $.ajax({
            url: "/PayrollAdmin/Payroll/SearchList",
            method: "Get",
            data: { name: name, date: date },
            success: function (res) {
                if (res.status == 200) {
                    $("#successPayrollList").html("");
                    $("#falsePayrollList").html("");
                    var span = document.createElement('span');
                    span.textContent = "Seçilmiş tarixdə maaşları verilmiş işçilər ";
                    $("#successPayrollList").append(span);
                    addNewPayrollList(res.data);
                }
                else if (res.status == 400) {
                    $("#falsePayrollList").html("");
                    $("#successPayrollList").html("");
                    var span = document.createElement('span');
                    span.textContent = "Seçilmiş tarixdə və ya seçdiyiniz adda maaşı verilmiş işçi yoxdur";
                    $("#falsePayrollList").append(span);
                    addNewPayrollList(res.data);
                    console.log(res.data);
                }
            },
            error: function (xhr) {
                console.log(xhr);
            }
        });
    })

    //@* --------- payrollSalary search -----------------------------------------------------------------------------------------------------*@

    function addNewPayrollSalary(datas) {
        var maintr = $("#trPayrollSalary").clone();
        $("table.payrollSalaryTable tbody").html(maintr);
        for (var data of datas) {
            var tr = $("#trPayrollSalary").clone();
            console.log(data);
            tr.removeClass("d-none");
            tr.removeAttr('id');
            tr.find("td:nth-child(1) input").val(data.id);
            tr.find("img").attr("src", "/img/" + data.image);
            tr.find("td:nth-child(3)").text(data.firstname);
            tr.find("td:nth-child(4)").text(data.lastname);
            tr.find("td:nth-child(5)").text(data.name);
            tr.find("td:nth-child(6)").text(data.positionName);
            tr.find("td:nth-child(7)").text(data.shopname);
            tr.find("td:nth-child(8)").text(data.salaryAmount);
            $("table.payrollSalaryTable tbody").append(tr);
        }
    }

    $(document).on("click", ".payrollSalarySearch", function (e) {
        e.preventDefault();
        var date = $("#payrollSalaryDate").val();
        $.ajax({
            url: "/PayrollAdmin/Payroll/SalarySearch",
            method: "Get",
            data: { date: date },
            success: function (res) {
                if (res.status == 200) {
                    $("#salaryDate").html("");
                    var span = document.createElement('span');
                    span.textContent = "Seçilmiş tarixdə bütün işçilərin maaşı ödənilib....";
                    $("#salaryDate").append(span);
                    addNewPayrollSalary(res.data);
                }
                else if (res.status == 400) {
                    $("#salaryDate").html("");
                    var span = document.createElement('span');
                    span.textContent = "Seçilmiş tarixdə aşağıdakı işçilərin maaşı ödənilməyib....";
                    $("#salaryDate").append(span);
                    addNewPayrollSalary(res.data);
                }
                else if (res.message == 400)
                {
                    alert("Seçilmiş tarixə maaş hesablana bilməz!!!")
                    addNewPayrollSalary(res.data);
                }
            }

        });
    })

    //@* --------- payroll calc salary  -----------------------------------------------------------------------------------------------------*@

    $(".createSavePayroll").click(function (e) {
        let idRecruitments = [];
        $(".payrollSalaryTable tbody tr").each(function (index, value) {
            if ($(value).find("[type='checkbox']").is(":checked")) {
                var recruitment = {
                    recruitmentid: $(value).find("[class='checkbox']:checked").data("id"),
                    reason: $(value).find("input[name='Continuity.Reason']").val(),
                    permissiontype: $(value).find("select[name='Continuity.PermissionType']").val(),
                    date: $("#date").val()
                };
                idRecruitments.push($(value).find("[type='checkbox']").val());
            }
        });
        console.log(idRecruitments);
        console.log($("#payrollSalaryDate").val());
        $.ajax({
            url: "/PayrollAdmin/Payroll/CalcSalary",
            data: { id: idRecruitments, date: $("#payrollSalaryDate").val() },
            type: "POST",
            success: function (respons) {
                if (respons.message == 200)
                {
                    alert("Əməliyyat uğurludur: Maaşı ödənildi...");
                    $(".salary-tbody").remove();
                }
            }
        });
    });

    //@*-------- vacation create ---------------------------------------------------------------------------------------------------*@

    let vacationRecruitmentId
    $(document).on("click", ".vacation", function (e) {
        $("#vacation").modal("show");
        vacationRecruitmentId = $(this).data("id");
        console.log(vacationRecruitmentId);
    });

    $(document).on("click", ".vacationSave", function (e) {
        let vacationWhenStarted = $("#vacationWhenStarted").val();
        let vacationWhenLeft = $("#vacationWhenLeft").val();
        console.log(vacationWhenStarted);
        console.log(vacationWhenLeft);
        if (vacationRecruitmentId && vacationWhenStarted && vacationWhenLeft) {
            var ajax = ({
                url: "/PayrollAdmin/Vacation/Create",
                data: { id: vacationRecruitmentId, whenStarted: vacationWhenStarted, whenLeft: vacationWhenLeft, },
                method: "Post",
                success: function (respons) {
                    if (respons.message == 200) {
                        alert("Əməliyyat uğurludur: İşçiyə məzuniyyət əlavə olundu...")
                    }
                }
            })
        }

        var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();
        if (antiForgeryToken) {
            ajax.headers = {};
            ajax.headers["X-XSRF-Token"] = antiForgeryToken;
        };

        $.ajax(ajax);
    });


    //@* --------- vacation search-------------------------------*@

    function addNewRecruitmentVacation(datas) {
        var maintr = $("#trVacation").clone();
        $("table.vacationClone tbody").html(maintr);
        for (var data of datas) {
            var tr = $("#trVacation").clone();
            tr.removeClass("d-none");
            tr.removeAttr('id');
            tr.find("img").attr("src", "/img/" + data.image);
            tr.find("td:nth-child(2)").text(data.firstname);
            tr.find("td:nth-child(3)").text(data.lastname);
            tr.find("td:nth-child(4)").text(data.email);
            tr.find("td:nth-child(5)").text(data.name);
            tr.find("td:nth-child(6)").text(data.positionName);
            tr.find("td:nth-child(7)").text("Məzuniyyət götürüb...")
            $("table.vacationClone tbody").append(tr);
        }
    }

    $(document).on("click", ".searchVacationRecruitment", function (e) {
        e.preventDefault();
        var name = $("#vacationRecruitmentName").val();
        $.ajax({
            url: "/PayrollAdmin/Ajax/VacationSearch",
            method: "Get",
            data: { name: name },
            success: function (res) {
                console.log(res.data);
                if (res.status == 200) {
                    $("#vacationRecruitmentError").html("");
                    $("#vacationRecruitmentSuccess").html("");
                    var span = document.createElement('span');
                    span.textContent = "Axtardığınız işçi bu il məzuniyyət götürüb.";
                    $("#vacationRecruitmentSuccess").append(span);
                    addNewRecruitmentVacation(res.data);
                }
                else if (res.status == 400) {
                    $("#vacationRecruitmentError").html("");
                    $("#vacationRecruitmentSuccess").append(span);
                    var span = document.createElement('span');
                    span.textContent = "Axtardığınız adda bu il məzuniyyət götürmüş işçi yoxdur!!!";
                    $("#vacationRecruitmentError").append(span);
                    addNewRecruitmentVacation(res.data);
                }
            }
        });
    })

});

