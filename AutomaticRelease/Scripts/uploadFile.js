var applicationPath = window.applicationPath === "" ? "" : window.applicationPath || "../../";
var GUID = WebUploader.Base.guid();//一个GUID
function uploadFile(picker, thelist, ctlBtn, fileType) {
    var $ = jQuery;
    var $list = $('#' + thelist);
    var uploader = WebUploader.create({
        fileNumLimit: 1,//上传数量限制                
        fileSingleSizeLimit: 5000 * 1024 * 1024,//限制上传单个文件大小
        // 选完文件后，是否自动上传。
        auto: false,
        // swf文件路径
        swf: applicationPath + 'Scripts/webuploader/Uploader.swf',

        // 文件接收服务端。
        server: applicationPath + 'UpLoadFile/UpLoadProcess',

        // 选择文件的按钮。可选。
        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
        pick: '#' + picker,
        formData: {
            guid: GUID//自定义参数，待会儿解释              
        },
        // 只允许选择上传指定文件。
        accept: {
            extensions: 'rar,zip',
            mimeTypes: 'rar/*'
        }
    });
    // 当有文件被添加进队列的时候
    uploader.on('fileQueued', function (file) {
        $list.append('<div id="' + file.id + '" class="item">' +
            '<h4 class="info">' + file.name + '</h4>' +
            '<p class="state">等待上传...</p>' +
        '</div>');
    });
    // 文件上传过程中创建进度条实时显示。
    uploader.on('uploadProgress', function (file, percentage) {
        var $li = $('#' + file.id),
    $percent = $li.find('.progress .progress-bar');

        // 避免重复创建
        if (!$percent.length) {
            $percent = $('<div class="progress progress-striped active">' +
              '<div class="progress-bar" role="progressbar" style="width: 0%">' +
              '</div><span class="text"></span>' +
            '</div>').appendTo($li).find('.progress-bar');
        }

        $li.find('p.state').text('上传中');
        $li.find("span.text").text(Math.round(percentage * 100) + '%');
        $percent.css('width', percentage * 100 + '%');
    });

    // 文件上传成功，给item添加成功class, 用样式标记上传成功。
    uploader.on('uploadSuccess', function (file, response) {             
            $('#' + file.id).find('p.state').text('解压成功');      
    });

    // 文件上传失败，显示上传出错。
    uploader.on('uploadError', function (file) {
        $('#' + file.id).find('p.state').text('上传出错');
    });

    // 完成上传完了，成功或者失败，先删除进度条。
    uploader.on('uploadComplete', function (file) {
        $('#' + file.id).find('.progress').fadeOut();
    });

    //所有文件上传完毕
    uploader.on("uploadFinished", function (block, data) {     
        //提交表单

    });
    //开始上传
    $('#' + ctlBtn).click(function () {      
        if ($("input[name='rdoGroup']:checked").length == 0) {
            alert("请选择项目分组");
            return;
        }
        var id = $("input[name='rdoGroup']:checked").val();
        if (id != "" && typeof (id) != "undefined"&&$("input[name='rdoGroup']:checked").attr("data-val") == "0") { 
                uploader.options.formData = { "fileType": fileType, "guid": GUID };
                uploader.options.formData.categoryPath = $('#webCategoryid').val();
                uploader.options.formData.webGroupId = id;
                uploader.upload();          
        } else {
            alert("请选择项目分组");
        }
       
    });
}