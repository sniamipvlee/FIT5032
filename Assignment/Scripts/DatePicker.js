$(function () {
    $(".datepicker").datepicker(
        option = {
            minDate: new Date(),
            showAnim: "fadeIn"
        }).bind('dateSelected', function (e, selectedDate, $td) {
            $("#hintMsg").html(selectedDate);
        });
});