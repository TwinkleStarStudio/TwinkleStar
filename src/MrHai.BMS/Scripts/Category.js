$("#ddCa").change(function () {
    var index = layer.load(1, {
        shade: [0.1, '#fff'] //0.1透明度的白色背景
    });
    $.get("/Category/WorkList" + "/" + this.value, function (data) {
        layer.close(index);
        $("#ddWork").empty();
        var opt = $("<option/>")
        opt.attr("value", "-1");
        opt.html("请选择");
        $("#ddWork").append(opt);
        if (data.result.errcode == 0) {
            for (var i = 0; i < data.body.length; i++) {
                var opt = $("<option/>")
                opt.attr("value", data.body[i].Id);
                opt.html(data.body[i].Name);
                $("#ddWork").append(opt);
            }
        }
        else {
            layer.msg(data.result.msg);
        }
    }, "json")
})