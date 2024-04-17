define(["/lib/knockout/knockout-min.js"], function (ko) {
    let _model;
    let _controllerName;
    let _currentLevel;
    let _levels;

    function getLevelArgument(currentVal) {
        let levelArgument;
        if (_currentLevel === 0) {
            levelArgument = '';
        }
        else if (_currentLevel === 1) {
            if (!currentVal)
                currentVal = _model.level0selected();
            levelArgument = currentVal;
        }
        else if (_currentLevel === 2) {
            if (!currentVal)
                currentVal = _model.level1selected();

            levelArgument = _model.level0selected() + ',' + currentVal;
        }
        else if (_currentLevel === 3) {
            if (!currentVal)
                currentVal = _model.level2selected();

            levelArgument = _model.level0selected() + ',' + _model.level1selected() + ',' + currentVal;
        }
        else
            alert('not implemented!!');
        return levelArgument;
    }

    function redirectSelected(data) {
        _model.newRedirect(data.name);
    }

    function addNew() {
        let data = {
            levels: getLevelArgument().split(','),
            lookups: _model.newLookups(),
            redirect: _model.newRedirect()
        };

        $.ajax({
            method: 'POST',
            url: '/' + _controllerName + '/AddLookup',
            dataType: 'json',
            data:data,
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    let dat = data[i];
                    _model.lookups.push(dat);
                    if (!isRedirect(dat))
                        _model.redirectLookups.push(dat);

                    if (_currentLevel === 0 && _model.level0lookups.indexOf(dat)<0) 
                        _model.level0lookups.push(dat);
                    else if (_currentLevel === 1 && _model.level1lookups.indexOf(dat)<0) 
                        _model.level1lookups.push(dat);
                    else if (_currentLevel === 2 && _model.level2lookups.indexOf(dat)<0)
                        _model.level2lookups.push(dat);
                }                

                _model.newLookups('');
                _model.newRedirect('');
            }
        });
    }

    function clickAction(level, value) {
        _currentLevel = level + 1;

        let levelArgument = getLevelArgument(value.name);
        

        $.ajax({
            method: 'GET',
            url: '/' + _controllerName + '/GetLevel?levels=' + levelArgument,
            success: function (data) {
                _model.lookups.removeAll();
                _model.lookups(data.values);
                _model.redirectLookups.removeAll();
                _model.redirectLookups(data.values.filter(function (r) { return !isRedirect(r); }));

                switch (_currentLevel) {      
                    case 1:
                        _model.level0selected(value.name);
                        _model.level1lookups.removeAll();
                        _model.level1lookups(data.values);
                        _model.level1selected(_levels.length > 1 ? data.levels[1] : '');
                        _model.level2lookups.removeAll();
                        _model.level2selected(_levels.length > 2 ? data.levels[2] : '');
                        break;
                    case 2:
                        _model.level1selected(value.name);
                        _model.level2lookups.removeAll();
                        _model.level2lookups(data.values);
                        _model.level2selected(_levels.length > 2 ? data.levels[2] : '');
                        break;
                    case 3:
                        _model.level2selected(value.name);                        
                }
            }
        });        
    }

    function deleteLookup(value) {
        let data = {
            levels: getLevelArgument().split(','),
            lookup: value.name
        };

        $.ajax({
            method: 'DELETE',
            url: '/' + _controllerName + '/DeleteLookup',
            dataType: 'json',
            data: data,
            success: function (data) {
                _model.lookups.remove(value);
                _model.redirectLookups.remove(value);
                switch (_currentLevel) {
                    case 1:                        
                        _model.level1lookups.remove(value);                        
                        _model.level1selected(_levels.length > 1 ? data.levels[1] : '');
                        _model.level2selected(_levels.length > 2 ? data.levels[2] : '');
                        break;
                    case 2:                        
                        _model.level2lookups.remove(value);                      
                        _model.level2selected(_levels.length > 2 ? data.levels[2] : '');
                        break;
                }
            }
        });  
    }

    function isRedirect(v) {
        return v.redirect;
    }


    function start(controllerName) {
        _controllerName = controllerName;

        $.ajax({
            method: 'GET',
            url: '/' + controllerName + '/GetLevel?levels=',
            success: function (data) {
                _levels = data.levels;

                _model = {
                    lookups: ko.observableArray(data.values),
                    redirectLookups: ko.observableArray(data.values.filter(function (r) { return !isRedirect(r); })),
                    newLookups: ko.observable(),
                    addNew: addNew,                    
                    level0selected: ko.observable(_levels.length>0? data.levels[0]:''),
                    level0lookups: ko.observableArray(data.values),
                    level1selected: ko.observable(_levels.length>1? data.levels[1]: ''),
                    level1lookups: ko.observableArray(),
                    level2selected: ko.observable(_levels.length>2? data.levels[2]:''),
                    level2lookups: ko.observableArray(),
                    clickAction0: function (a) {
                        clickAction(0, a);
                    },
                    clickAction1: function (a) {
                        clickAction(1, a);
                    },
                    clickAction2: function (a) {
                        clickAction(2, a);
                    },
                    deleteLookup: deleteLookup,
                    newRedirect: ko.observable(),
                    redirectSelected: redirectSelected
                };
                _currentLevel = 0;
                ko.applyBindings(_model);
            }
        });
    }

    return {
        start: start
    };
});