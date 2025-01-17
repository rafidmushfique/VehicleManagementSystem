// Write your JavaScript code.


//Code for Modal 
$(function () {
    var placeholderElement = $('#modal-placeholder');

    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });
    });

    placeholderElement.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataToSend = form.serialize();

        $.post(actionUrl, dataToSend).done(function (data) {
            var newBody = $('.modal-body', data);
            placeholderElement.find('.modal-body').replaceWith(newBody);

            var isValid = newBody.find('[name="IsValid"]').val() == 'True';
            if (isValid) {
                placeholderElement.find('.modal').modal('hide');
            }
        });
    });



    $('#ManufacMachineCode').on('change', function () {
        var machineCode = $('#ManufacMachineCode').val();
        var requisitionNo = $('#RequisitionNo').val();
        if (machineCode != '') {
            $.ajax({
                type: "POST",
                url: "/Production/GetMachineCapacity",
                data: { machineCode: machineCode, requisitionNo: requisitionNo },
                success: function (result) {
                    $('#ManufacMachineCapacity').val();
                    $('#ManufacMachineCapacity').val(result);
                },
                error: function () {
                    $('#ManufacMachineCapacity').val();
                }
            });
        };
    });

    $('#PackMachineCode').on('change', function () {
        var machineCode = $('#PackMachineCode').val();
        var requisitionNo = $('#RequisitionNo').val();
        if (machineCode != '') {
            $.ajax({
                type: "POST",
                url: "/Production/GetMachineCapacityPacking",
                data: { machineCode: machineCode, requisitionNo: requisitionNo },
                success: function (result) {
                    $('#PackMachineCapacity').val();
                    $('#PackMachineCapacity').val(result);
                },
                error: function () {
                    $('#PackMachineCapacity').val();
                }
            });
        };
    });

    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode != 46 && (charCode < 48 || charCode > 57)))
            return false;
        return true;
    }
    

});

//Function: 
