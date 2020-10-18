Callreport.prototype.Employee = new Employee(baseObj);
var table;
var array = [];
//var selectedDept1 = [];


function department(Id, EmployeeName, departmentname) {
    this.Id = Id;
    this.EmployeeName = EmployeeName;
    this.departmentname = departmentname;
}


function Employee(baseObj) {
    var EmpObj = this;
    EmpObj.selectedDept1 = [];
    EmpObj.finalselectres = [];

    EmpObj.LoadGrid = function () {
        if (table != null) {
            table.destroy();
        }

        table = $("#commontbl").DataTable({
            "paging": true,
            "processing": true,
            "serverSide": true,
            "autoWidth": false,
            "sDom": "ltipr", // hide default search box 
            "ajax": {
                url: "/Home/EmployeeGrid",
                type: 'POST',
                async: true

            },
            "columns": [

                { "data": "Id", "title": "Employee Id", "visible": false },
                { "data": "EmployeeName", "title": "Employee Name", "visible": true },
                { "data": "EmployeeAddress", "title": "Employee Address", "visible": true },
                { "data": "DepartmentName", "title": "Department Name", "visible": true },

            ],

            aoColumnDefs: [
                {
                    "orderable": false, "targets": 4,
                    "aTargets": [4],
                    "title": "Actions", "visible": true,
                    "width": "8%",
                    "mRender": function (data, type, row) {
                        var returnValue = '<div style="text-align:center">';
                        returnValue = returnValue + '<a href="' + '/Home/EditEmployee/' + row["Id"] + '"><i data-toggle="tooltip" data-placement="top" title="Edit" class="fa fa-edit fa-cog editGrid" aria-hidden="true"</i></a>';
                        returnValue = returnValue + '</div>';
                        return returnValue;
                    }
                }
            ],

        });

        $("#searchButton").on("click", function () {
            var v = $("#tbl_txt_search").val();
            table.search(v).draw();
        });

        $("#tbl_txt_search").keyup(function (e) {
            if (e.keyCode == 13) {
                table.search($(this).val()).draw();
            }
        });

        $("#commontbl tbody").on("click", "tr", ".editGrid", function () {
            debugger;
            var data = table.row(this).data();
            //EmpObj.EditEmp(data.Id);
        });


        $("#commontbl tbody").on("click", "tr", function () {
            debugger;
            var data = table.row(this).data();
            var EmpId = data.Id;
            var EmpName = data.EmployeeName;
            var deptname = data.DepartmentName;
            var selectedDept = deptname;
            // $("#selectedDept").empty();
            //if ($(this).hasClass("selectedRow")) {
            //}
            //else {
            //    $(this).removeClass("selectedRow");
            //   // EmpObj.selectedDept1 = [];
            //}
            var result = $.grep(EmpObj.selectedDept1, function (e, v) {
                return e.Id == EmpId
            });
            if (result.length === 0) {
                EmpObj.selectedDept1.push(new department(data.Id, data.EmployeeName, data.DepartmentName));
                $(this).toggleClass("selectedRow");
            }

            else if (result.length === 1) {
                EmpObj.selectedDept1 = $.grep(EmpObj.selectedDept1, function (e, v) {
                    return e.Id != EmpId;
                });
                $(this).toggleClass("selectedRow");
            }


            var resobj = EmpObj.selectedDept1;

            $("#selectedDept").empty();

            var html = "<ul>";
            $.each(EmpObj.selectedDept1, function (e, i) {

                html += "<li class='final-selected-dept'>" + i.EmployeeName + "<span aria-hidden='true' style='margin-left:39%; color:red; text-decoration: underline;background: white; cursor: pointer;' class='cancelbtn' >X</span></li>";
            });
            html += "</ul>";
            $("#selectedDept").append(html);
        });


        EmpObj.EditEmp = function (Id) {
            //$.ajax({
            //    url: "/Home/EditEmployee/" + Id,
            //    type: "POST",
            //    success: function (data) {
            //        debugger;
            //        $("#myModalBodyDiv1").html(data);
            //        $("#myModal1").modal("show");
            //    },
            //    error: function (err) {
            //        alert(err);
            //    }
            //});


            var url = "/Home/EditEmployee/" + Id
            $("#myModalBodyDiv1").load(url, function () {
                $("#myModal1").modal({
                    backdrop: 'static',
                    keyboard: false
                });
                $("#myModal1").modal("show");
            })
        }
    }
}

baseObj.Employee.LoadGrid();


$(document).on("click", ".cancelbtn", function () {
    debugger;
    //stuff to happen
    var res = $(this).parent().text().slice(0, -1);
    //$("#selectedDept").empty();

    $(this).closest('li').toggleClass('strike').fadeOut('slow', function () { $(this).remove(); });
    //EmpObj.selectedDept1 = $.grep(EmpObj.selectedDept1, function (e) {
    //    e.Id != EmpId;
    //});
});


//$.sessionTimeout({

//    // custom warning message
//    message: 'Your session is about to expire.',

//    // keep alive url
//    keepAliveUrl: '/keep-alive',

//    // request type
//    keepAliveAjaxRequestType: 'POST',

//    // redirect url
//    redirUrl: '/timed-out',

//    // logout url
//    logoutUrl: '/log-out',

//    // 15 minutes
//    warnAfter: 900000,

//    // 20 minutes
//    redirAfter: 1200000,

//    // appends time stamp to keep alive url to prevent caching
//    appendTime: true

//});






