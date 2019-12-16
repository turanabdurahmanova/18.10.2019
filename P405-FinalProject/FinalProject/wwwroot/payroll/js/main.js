$(document).ready(function () {

    $(".resp-tab-item").click(function () {

        $(".resp-tab-item").removeClass('resp-tab-active')
        $(this).addClass('resp-tab-active')

        var tab_id = $(this).attr('data-tab');

        $('.tab-content').removeClass('current');

        $("#" + tab_id).addClass('current');
    });

    $("#addformerwork").click(function () {
        var counter = $("#formerworkCount").val();
        let html = $("#formerwork").clone();
        html.removeAttr('id');
        html.find(".removeFormerWork").click(function (e) {
            $(e.target).parent().remove();
        });
        var inputs = html.find("[name^='FormerWorks']");
        for (var input of inputs) {
            var inputName = $(input).attr("name").replace("0", counter);
            $(input).attr("name", inputName);
        }
        $("#formerworkCount").val(++counter)
        $("#buraEleveEt").before(html);
        html.toggle("slow");
    });

    $("#addrecruitment").click(function () {
        $("#recruitment").toggle("slow", function () {

        });
    });

    $('[data-toggle="popover"]').popover();
   
    $(function () {
        var $loader = $('#loader'),
            max = 75, speed = 500,
            char = '<i>.</i>', count = 0,
            dots = function () {
                if (count <= max) {
                    count++;
                    for (var i = 0; i < 1; i++) {
                        $loader.append(char);
                        $loader.find('i').fadeIn(speed);
                    }
                } else {
                    clearInterval(dots);
                }
            };
        setInterval(dots, speed / 2);
    });
})
