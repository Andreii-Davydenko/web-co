var colorList = ["#ffffcc", "#ffff99", "#ffff4d", "#ffcc00", "#ff9933",
    "#ff6600", "#c44f02", "#f21e88", "#fe3f9f", "#ff5cad",
    "#ff66ff", "#ff99ff", "#ffc8ff", "#ffdeff", "#bb78fd",
    "#9966ff", "#057ece", "#0099ff", "#00ccff", "#60daf9",
    "#99ff99", "#65f965", "#3cd53c", "#8fcf10", "#b7ee10",
    "#e6bc3d", "#cba226", "#a3a34f", "#b5b588", "#6deec0",
    "#27ce92", "#089f69", "#10989a", "#9b04b6", "#134bed",
    "#929292", "#bcbcbc", "#b56565", "#a57c0f", "#ffa3b1",
    "#6d6d6d", "#735353", "#1d8727", "#7e5e08", "#455896",
    "#58660c", "#7140e2", "#00ff12", "#017b7a", "#267142",

    "#ffffcc", "#ffff99", "#ffff4d", "#ffcc00", "#ff9933",
    "#ff6600", "#c44f02", "#f21e88", "#fe3f9f", "#ff5cad",
    "#ff66ff", "#ff99ff", "#ffc8ff", "#ffdeff", "#bb78fd",
    "#9966ff", "#057ece", "#0099ff", "#00ccff", "#60daf9",
    "#99ff99", "#65f965", "#3cd53c", "#8fcf10", "#b7ee10",
    "#e6bc3d", "#cba226", "#a3a34f", "#b5b588", "#6deec0",
    "#27ce92", "#089f69", "#10989a", "#9b04b6", "#134bed",
    "#929292", "#bcbcbc", "#b56565", "#a57c0f", "#ffa3b1",
    "#6d6d6d", "#735353", "#1d8727", "#7e5e08", "#455896",
    "#58660c", "#7140e2", "#00ff12", "#017b7a", "#267142"
];

$(function () {
    initializeButton();
    initializeXmlFields();
    calculateChildsToCouple(true);
    setParentsCompleted();

    initializePresetMatches();

    //$("#CustomerPrefix").html(getCurrentCustomer());
    var automap = $("#automap");
    if (typeof (Storage) !== "undefined") {
        if (sessionStorage.automap === "true") {
            automap.attr("checked", "checked");
        }
    }

    function storeAutomap() {
        if (typeof (Storage) !== "undefined") {
            sessionStorage.automap = automap.prop("checked");
        }
    }

    automap.click(storeAutomap);
});


function decode(encoded) {
    var elem = document.createElement('textarea');
    elem.innerHTML = encoded;
    return elem.value;
}

function clickFor(fullPath, dbTableName, dbName) {
    fullPath = decode(fullPath);

    $(".xml-block-content").filter(function () {
        return $(this).attr('data-full-path').toLowerCase() == fullPath;
    }).each(function () {

        var dbButton = $(".db-button[data-db-table-name='" + dbTableName + "'][data-db-name='" + dbName + "']");
        coupleDatabaseFieldsNoCoupling(dbButton, this);

    });
}

function initializeButton() {
    $(".multiple-button").data("color", "#FF0000");
    $(".db-button").each(function (i, o) {
        // Calculate position in colorList to enable reusing colors.
        var difference = (i / colorList.length);
        difference = difference - Math.floor(difference);
        $(this).data("color", colorList[colorList.length * difference]);
    });

    $(".db-button, .multiple-button").click(function () {
        buttonClicked(this);
    });

    $("#SubmitMapping").click(function () {
        startSaving();
        saveMapping();
    });

    $("#SelectRepeatingElement").click(function () {
        if ($(this).hasClass("selected")) {
            $(this).removeClass("selected");
        } else {
            $(this).addClass("selected");
        }
    });

    $("#RemoveSelectedRepeatingElement").click(function () {
        $("#SelectedRepeatingElementContainer").hide();
        $("#SelectedRepeatingElement").html("");
        $("#SelectedRepeatingElement").removeData("full-path");
    });
}

function startSaving() {
    $("#loader").show();
    $(".content-container").hide();
    $("#SubmitMapping").hide();
}

function endSaving(result) {
    $("#loader").hide();
    $("#result").data();
    $("#result").html(result);
}

function saveMapping() {

    var coupledData = [];

    $(".xml-block-content.no-child").each(function () {
        var fullPath = $(this).data("full-path");
        var dbTableName = $(this).attr("data-db-table-name");
        var dbName = $(this).attr("data-db-name");

        if (dbName && dbName.indexOf('|') > 0) {
            var splitNames = dbName.split('|');
            var splitTables = dbTableName.split('|');

            for (var i = 0; i < splitNames.length; i++) {
                let dbNameSplit = splitNames[i];
                let dbTableNameSplit = splitTables[i];
                let item = { Path: fullPath, DbName: dbNameSplit, DbTableName: dbTableNameSplit };
                coupledData.push(item);
            }
        }
        else {
            let item = { Path: fullPath, DbName: dbName, DbTableName: dbTableName };
            coupledData.push(item);
        }

    });


    var repeatingElement = $("#SelectedRepeatingElement").html();


    var totalData = { result: { RepeatingNodes: repeatingElement, Host: host, Data: coupledData, CustomerPrefixName: customerPrefixName, HashBase64: hashBase64 } };

    $.ajax({
        type: 'POST',
        url: 'SaveMapping',
        data: totalData,
        dataType: "json",
        success: function () {
            location.href = 'MapManual';
        },
        fail: function (json) {
            endSaving("Er ging iets onverwacht mis. " + json);
        }
    });
}

function xmlButtonClick(my) {
    $(".xml-block-content").removeClass("selected");
    $(my).addClass("selected");
}

function xmlButtonDoubleClick(my) {
    $(".xml-block-content").removeClass("selected");

    var currentButton = $(my);
    currentButton.addClass("selected");

    var xmlBlock = $(".xml-block-content.selected");
    coupleDatabaseFieldsNoCoupling(my, xmlBlock);

    currentButton.removeAttr("data-db-name");
    currentButton.removeAttr("data-db-table-name");
    currentButton.css("background-color", "Red");

    calculateChildsToCouple(true);
    setParentsCompleted();
}

function initializeXmlFields() {
    $(".xml-block-content.no-child").click(function () {
        xmlButtonClick(this);
    });

    $(".xml-block-content.no-child").dblclick(function () {
        //set as ignore

        xmlButtonDoubleClick(this);
    });

    $(".xml-block-content.with-child").click(function () {
        if ($("#SelectRepeatingElement.selected").length == 1) {
            $("#SelectRepeatingElement").removeClass("selected");
            $("#SelectedRepeatingElementContainer").show();

            var currentSelectedHtml = $("#SelectedRepeatingElement").html();
            if (currentSelectedHtml.length > 0)
                currentSelectedHtml += ', ';
            currentSelectedHtml += $(this).find(".xml-block-name").html();
            //$(this).find(".xml-block-name").html()
            $("#SelectedRepeatingElement").html(currentSelectedHtml);

            var fullPathData = $("#SelectedRepeatingElement").data("full-path");
            if (fullPathData) {
                if (fullPathData.length > 0)
                    fullPathData += '*$*';
                fullPathData += $(this).data("full-path");
                $("#SelectedRepeatingElement").data("full-path", fullPathData);
            }
        } else {
            clickOnParentItem($(this));
        }
    });
}

function clickOnParentItem(my) {
    if ($(my).hasClass("closed")) {
        let icon = $(my).find(".xml-block-icon");
        icon.addClass("glyphicon-minus");
        icon.removeClass("glyphicon-plus");
        $(my).removeClass("closed");
        $(my).next().slideDown();
    } else {
        let icon = $(my).find(".xml-block-icon");
        icon.removeClass("glyphicon-minus");
        icon.addClass("glyphicon-plus");
        $(my).addClass("closed");
        $(my).next().slideUp();
    }
}

function buttonClicked(button) {
    var xmlBlock = $(".xml-block-content.selected");
    if (xmlBlock.length == 1) {
        coupleDatabaseFieldToXmlField(button, xmlBlock);
    }
}

function coupleDatabaseFieldToXmlField(dbButton, xmlBlock) {
    coupleDatabaseFieldsNoCoupling(dbButton, xmlBlock);

    calculateChildsToCouple(false);
    setParentsCompleted();
}

function ignoreFor(fullPath) {
    fullPath = decode(fullPath);

    $(".xml-block-content").filter(function () {
        return $(this).attr('data-full-path').toLowerCase() == fullPath;
    }).each(function () {
        var ignoreBtn = $("#ignoreButton");
        coupleDatabaseFieldsNoCoupling(ignoreBtn, this);

        calculateChildsToCouple(true);
        setParentsCompleted();
    });
}

var _multiEnabled = false;
var _presetRunning = false;

function coupleDatabaseFieldsNoCoupling(dbButton, xmlBlock) {
    var color = $(dbButton).data("color");
    if (!color)
        color = 'red';
    var dbName = $(dbButton).attr("data-db-name");
    var dbTableName = $(dbButton).attr("data-db-table-name");

    _multiEnabled = $("#multiselect").prop("checked");
    if (!_multiEnabled && !_presetRunning) {
        var oldDbName = xmlBlock.attr("data-db-name");
        var oldDbTableName = xmlBlock.attr("data-db-table-name");

        if (oldDbName) {
            var oldDbNameParts = oldDbName.split('|');
            var oldDbTableNameParts = oldDbTableName.split('|');

            for (var i = 0; i < oldDbNameParts.length; i++) {
                setCurrentlyConnectedBlockToDefault(oldDbNameParts[i], oldDbTableNameParts[i]);
            }
        }
    }

    $(dbButton).css("background-color", color);

    var fullPath = $(xmlBlock).data("full-path");
    $(xmlBlock).removeClass("selected");
    setXmlBlocksWithSamePathToColor(color, dbName, dbTableName, fullPath);
}

function calculateChildsToCouple(load) {
    var fullPaths = getNotCoupledPaths();
    var unique = fullPaths.filter(function (item, i, ar) { return ar.indexOf(item) === i; });
    var length = unique.length;

    $("#AmountToMap").html(length);
    if (length == 0) {
        var submitMapping = $("#SubmitMapping");
        submitMapping.show();

        if (load) {

            if (typeof (Storage) !== "undefined") {
                if (sessionStorage.automap == "true")
                    submitMapping.click();
            }
        }
    }
}

function getNotCoupledPaths() {
    var fullPaths = [];
    $(".xml-block-content.no-child").each(function () {
        if (!$(this).data("coupled")) {
            var fullPath = $(this).data("full-path");
            fullPaths.push(fullPath);
        }
    });
    return fullPaths;
}

function setParentsCompleted() {
    $(".xml-block-content.with-child").each(function () {
        var isCompleted = true;
        $(this).parent().find(".xml-block-content.no-child").each(function () {
            if (!($(this).data("coupled") == true)) {
                isCompleted = false;
            }
        });
        if (isCompleted) {
            $(this).css("background-color", "#06e23d");
            $(this).css("color", "white");
            $(this).css("border", "0px solid #00cb33");
            $(this).css("border-left", "30px solid #00cb33");
        }
    });
}

function setXmlBlocksWithSamePathToColor(color, dbName, dbTableName, fullPath) {
    $(".xml-block-content[data-full-path='" + fullPath + "']").each(function () {
        $(this).css("background-color", color);

        if (_multiEnabled || _presetRunning) {

            var currentDataName = $(this).attr("data-db-name");
            if (currentDataName && currentDataName.length > 0 && dbName && dbName.indexOf(currentDataName + '|') < 0 && dbName !== currentDataName) {
                dbName = currentDataName + "|" + dbName;

                var currentTableName = $(this).attr("data-db-table-name");
                if (currentTableName && currentTableName.length > 0)
                    dbTableName = currentTableName + "|" + dbTableName;
            }
        }

        $(this).attr("data-db-name", dbName);
        $(this).attr("data-db-table-name", dbTableName);

        $(this).data("coupled", true);
    });
}

function setCurrentlyConnectedBlockToDefault(dbName, dbTableName) {
    var elem = $("*[data-db-name='" + dbName + "'][data-db-table-name='" + dbTableName + "']");
    $(elem).removeAttr("data-db-name");
    $(elem).removeAttr("data-db-table-name");
    $(elem).data("coupled", false);
    $(elem).css("background-color", "#FFFFFF");
}