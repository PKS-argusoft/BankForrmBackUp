var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#templateTblData').DataTable({
        "ajax": {   "url": "/Admin/Template/GetAll"
            },
        "columns": [
            
            { "data": "templateName", "width": "30%" },
            {"data" : "order" , "width":"20%"},
            {
                "data": "id",
                "render": function (data) {
                    return `
                           
                            <a class="bg-transparent border-0 " style="font-size:1.5rem; color:green;" ><i class="bi bi-caret-up-fill"></i></a>
                          
                         
                            <a class="bg-transparent border-0 " style="font-size:1.5rem; color:red;"  ><i class="bi bi-caret-down-fill"></i></a>  
                          
                    `                                                            
                }, "width":"30%"

            }
        ]
    });
    return dataTable;
}