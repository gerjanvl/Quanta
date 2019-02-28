(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["main"],{

/***/ "./src/$$_lazy_route_resource lazy recursive":
/*!**********************************************************!*\
  !*** ./src/$$_lazy_route_resource lazy namespace object ***!
  \**********************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

function webpackEmptyAsyncContext(req) {
	// Here Promise.resolve().then() is used instead of new Promise() to prevent
	// uncaught exception popping up in devtools
	return Promise.resolve().then(function() {
		var e = new Error("Cannot find module '" + req + "'");
		e.code = 'MODULE_NOT_FOUND';
		throw e;
	});
}
webpackEmptyAsyncContext.keys = function() { return []; };
webpackEmptyAsyncContext.resolve = webpackEmptyAsyncContext;
module.exports = webpackEmptyAsyncContext;
webpackEmptyAsyncContext.id = "./src/$$_lazy_route_resource lazy recursive";

/***/ }),

/***/ "./src/app/app-routing.module.ts":
/*!***************************************!*\
  !*** ./src/app/app-routing.module.ts ***!
  \***************************************/
/*! exports provided: AppRoutingModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AppRoutingModule", function() { return AppRoutingModule; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/fesm5/router.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var microsoft_adal_angular6__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! microsoft-adal-angular6 */ "./node_modules/microsoft-adal-angular6/fesm5/microsoft-adal-angular6.js");
/* harmony import */ var _app_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./app.component */ "./src/app/app.component.ts");





var routes = [
    { path: '', component: _app_component__WEBPACK_IMPORTED_MODULE_4__["AppComponent"], pathMatch: 'full', canActivate: [microsoft_adal_angular6__WEBPACK_IMPORTED_MODULE_3__["AuthenticationGuard"]] }
];
var AppRoutingModule = /** @class */ (function () {
    function AppRoutingModule() {
    }
    AppRoutingModule = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_2__["NgModule"])({
            imports: [
                _angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"].forRoot(routes),
            ],
            exports: [
                _angular_router__WEBPACK_IMPORTED_MODULE_1__["RouterModule"]
            ]
        })
    ], AppRoutingModule);
    return AppRoutingModule;
}());



/***/ }),

/***/ "./src/app/app.component.html":
/*!************************************!*\
  !*** ./src/app/app.component.html ***!
  \************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<div #displayElement class=\"display\">\n\n</div>\n"

/***/ }),

/***/ "./src/app/app.component.scss":
/*!************************************!*\
  !*** ./src/app/app.component.scss ***!
  \************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ".display {\n  position: absolute;\n  width: 100%;\n  height: 100%; }\n\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvQzpcXFVzZXJzXFxnZXJqYVxcRGVza3RvcFxcZ3VhY2Ftb2xlLWFuZ3VsYXIvc3JjXFxhcHBcXGFwcC5jb21wb25lbnQuc2NzcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQTtFQUNFLGtCQUFrQjtFQUNsQixXQUFXO0VBQ1gsWUFBWSxFQUFBIiwiZmlsZSI6InNyYy9hcHAvYXBwLmNvbXBvbmVudC5zY3NzIiwic291cmNlc0NvbnRlbnQiOlsiLmRpc3BsYXkge1xyXG4gIHBvc2l0aW9uOiBhYnNvbHV0ZTtcclxuICB3aWR0aDogMTAwJTtcclxuICBoZWlnaHQ6IDEwMCU7XHJcbn1cclxuIl19 */"

/***/ }),

/***/ "./src/app/app.component.ts":
/*!**********************************!*\
  !*** ./src/app/app.component.ts ***!
  \**********************************/
/*! exports provided: AppComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AppComponent", function() { return AppComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/fesm5/http.js");
/* harmony import */ var guacamole_common_js__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! guacamole-common-js */ "./node_modules/guacamole-common-js/dist/guacamole-common.js");
/* harmony import */ var guacamole_common_js__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(guacamole_common_js__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _common_signalr_tunnel__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./common/signalr.tunnel */ "./src/app/common/signalr.tunnel.ts");
/* harmony import */ var microsoft_adal_angular6__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! microsoft-adal-angular6 */ "./node_modules/microsoft-adal-angular6/fesm5/microsoft-adal-angular6.js");






var AppComponent = /** @class */ (function () {
    function AppComponent(adalSvc, httpClient) {
        this.adalSvc = adalSvc;
        this.title = "guacamole-angular";
        httpClient
            .get("https://localhost:44308/api/v1.0/me/devices", {
            headers: { Authorization: "Bearer " + this.adalSvc.accessToken }
        })
            .subscribe(function (o) {
            console.log(o);
        });
    }
    AppComponent.prototype.getGraphApiToken = function () {
        return this.adalSvc
            .acquireToken("https://graph.microsoft.com")
            .toPromise();
    };
    AppComponent.prototype.getTunnel = function () {
        var _this = this;
        return new _common_signalr_tunnel__WEBPACK_IMPORTED_MODULE_4__["SignalRTunnel"](function (builder) {
            builder.withUrl("https://localhost:44308/ws/", {
                accessTokenFactory: function () { return _this.adalSvc.accessToken; }
            });
            return builder;
        });
    };
    AppComponent.prototype.connect = function () {
        var client = new guacamole_common_js__WEBPACK_IMPORTED_MODULE_3___default.a.Client(this.getTunnel());
        this.displayElement.nativeElement.appendChild(client.getDisplay().getElement());
        var mouse = new guacamole_common_js__WEBPACK_IMPORTED_MODULE_3___default.a.Mouse(client.getDisplay().getElement());
        var display = client.getDisplay();
        mouse.onmousedown = mouse.onmouseup = mouse.onmousemove = function (mouseState) {
            client.sendMouseState(new guacamole_common_js__WEBPACK_IMPORTED_MODULE_3___default.a.Mouse.State(mouseState.x / display.getScale(), mouseState.y / display.getScale(), mouseState.left, mouseState.middle, mouseState.right, mouseState.up, mouseState.down));
        };
        display.getElement().onclick = function (e) {
            e.preventDefault();
            return false;
        };
        var keyboard = new guacamole_common_js__WEBPACK_IMPORTED_MODULE_3___default.a.Keyboard(document);
        keyboard.onkeydown = function (keysym) {
            client.sendKeyEvent(1, keysym);
        };
        keyboard.onkeyup = function (keysym) {
            client.sendKeyEvent(0, keysym);
        };
        client.onerror = function (error) {
            console.error(error);
        };
        client.connect({ deviceId: 1 });
    };
    AppComponent.prototype.ngOnInit = function () {
        this.connect();
    };
    tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"])("displayElement"),
        tslib__WEBPACK_IMPORTED_MODULE_0__["__metadata"]("design:type", _angular_core__WEBPACK_IMPORTED_MODULE_1__["ElementRef"])
    ], AppComponent.prototype, "displayElement", void 0);
    AppComponent = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"])({
            selector: "app-root",
            template: __webpack_require__(/*! ./app.component.html */ "./src/app/app.component.html"),
            styles: [__webpack_require__(/*! ./app.component.scss */ "./src/app/app.component.scss")]
        }),
        tslib__WEBPACK_IMPORTED_MODULE_0__["__metadata"]("design:paramtypes", [microsoft_adal_angular6__WEBPACK_IMPORTED_MODULE_5__["MsAdalAngular6Service"], _angular_common_http__WEBPACK_IMPORTED_MODULE_2__["HttpClient"]])
    ], AppComponent);
    return AppComponent;
}());



/***/ }),

/***/ "./src/app/app.module.ts":
/*!*******************************!*\
  !*** ./src/app/app.module.ts ***!
  \*******************************/
/*! exports provided: AppModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AppModule", function() { return AppModule; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_platform_browser__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/platform-browser */ "./node_modules/@angular/platform-browser/fesm5/platform-browser.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var microsoft_adal_angular6__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! microsoft-adal-angular6 */ "./node_modules/microsoft-adal-angular6/fesm5/microsoft-adal-angular6.js");
/* harmony import */ var _app_routing_module__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./app-routing.module */ "./src/app/app-routing.module.ts");
/* harmony import */ var _app_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./app.component */ "./src/app/app.component.ts");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/fesm5/http.js");







var AppModule = /** @class */ (function () {
    function AppModule() {
    }
    AppModule = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_2__["NgModule"])({
            declarations: [
                _app_component__WEBPACK_IMPORTED_MODULE_5__["AppComponent"]
            ],
            imports: [
                _angular_platform_browser__WEBPACK_IMPORTED_MODULE_1__["BrowserModule"],
                _app_routing_module__WEBPACK_IMPORTED_MODULE_4__["AppRoutingModule"],
                _angular_common_http__WEBPACK_IMPORTED_MODULE_6__["HttpClientModule"],
                microsoft_adal_angular6__WEBPACK_IMPORTED_MODULE_3__["MsAdalAngular6Module"].forRoot({
                    tenant: 'b2461714-367f-46fc-888c-6a50d99ec8b4',
                    clientId: '9c66cf46-331f-429f-b27b-e301c09e5bf5',
                    redirectUri: window.location.origin,
                    endpoints: {
                        "https://localhost:44308/api/v1.0/": "9c66cf46-331f-429f-b27b-e301c09e5bf5",
                    },
                    navigateToLoginRequestUrl: false,
                    cacheLocation: 'localStorage'
                }),
            ],
            providers: [microsoft_adal_angular6__WEBPACK_IMPORTED_MODULE_3__["AuthenticationGuard"]],
            bootstrap: [_app_component__WEBPACK_IMPORTED_MODULE_5__["AppComponent"]]
        })
    ], AppModule);
    return AppModule;
}());



/***/ }),

/***/ "./src/app/common/signalr.tunnel.ts":
/*!******************************************!*\
  !*** ./src/app/common/signalr.tunnel.ts ***!
  \******************************************/
/*! exports provided: SignalRTunnel */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SignalRTunnel", function() { return SignalRTunnel; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var guacamole_common_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! guacamole-common-js */ "./node_modules/guacamole-common-js/dist/guacamole-common.js");
/* harmony import */ var guacamole_common_js__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(guacamole_common_js__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _aspnet_signalr__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @aspnet/signalr */ "./node_modules/@aspnet/signalr/dist/esm/index.js");



var SignalRTunnel = /** @class */ (function (_super) {
    tslib__WEBPACK_IMPORTED_MODULE_0__["__extends"](SignalRTunnel, _super);
    function SignalRTunnel(builder) {
        var _this = _super.call(this) || this;
        _this.connect = function (data) {
            _this.setState(guacamole_common_js__WEBPACK_IMPORTED_MODULE_1___default.a.Tunnel.State.CONNECTING);
            _this._connection.start()
                .then(function () {
                _this._connection.on('connected', function (connectionId) {
                    _this.uuid = connectionId;
                    _this.setState(guacamole_common_js__WEBPACK_IMPORTED_MODULE_1___default.a.Tunnel.State.OPEN);
                });
                _this._connection.on('instruction', function (instruction) {
                    _this.oninstruction(instruction.opCode, instruction.args);
                });
                _this._connection.send('connect', data);
            }).catch(function (err) {
                _this.setState(guacamole_common_js__WEBPACK_IMPORTED_MODULE_1___default.a.Tunnel.State.CLOSED);
                console.error(err);
            });
        };
        _this.disconnect = function () {
            _this._connection.stop().then(function () {
                _this.setState(guacamole_common_js__WEBPACK_IMPORTED_MODULE_1___default.a.Tunnel.State.CLOSED);
            });
        };
        _this.sendMessage = function (opCode) {
            var args = [];
            for (var _i = 1; _i < arguments.length; _i++) {
                args[_i - 1] = arguments[_i];
            }
            _this._connection.send('WriteInstruction', { opCode: opCode, args: args.map(function (o) { return o.toString(); }) });
        };
        _this._connection = builder(new _aspnet_signalr__WEBPACK_IMPORTED_MODULE_2__["HubConnectionBuilder"]()).build();
        return _this;
    }
    SignalRTunnel.prototype.closeTunnel = function (status) {
        window.clearTimeout(this._receiveTimeoutHandle);
        window.clearTimeout(this._unstableTimeoutHandle);
        if (this.state === guacamole_common_js__WEBPACK_IMPORTED_MODULE_1___default.a.Tunnel.State.CLOSED) {
            return;
        }
        if (status.code !== guacamole_common_js__WEBPACK_IMPORTED_MODULE_1___default.a.Status.Code.SUCCESS && this.onerror) {
            this.onerror(status);
        }
        this.disconnect();
    };
    SignalRTunnel.prototype.resetTimeout = function () {
        window.clearTimeout(this._receiveTimeoutHandle);
        window.clearTimeout(this._unstableTimeoutHandle);
        if (this.state === guacamole_common_js__WEBPACK_IMPORTED_MODULE_1___default.a.Tunnel.State.UNSTABLE) {
            this.setState(guacamole_common_js__WEBPACK_IMPORTED_MODULE_1___default.a.Tunnel.State.OPEN);
        }
        this._receiveTimeoutHandle = window.setTimeout(function () {
            this.closeTunnel(new guacamole_common_js__WEBPACK_IMPORTED_MODULE_1___default.a.Status(guacamole_common_js__WEBPACK_IMPORTED_MODULE_1___default.a.Status.Code.UPSTREAM_TIMEOUT, 'Server timeout.'));
        }, this.receiveTimeout);
        this._unstableTimeoutHandle = window.setTimeout(function () {
            this.setState(guacamole_common_js__WEBPACK_IMPORTED_MODULE_1___default.a.Tunnel.State.UNSTABLE);
        }, this.unstableThreshold);
    };
    return SignalRTunnel;
}(guacamole_common_js__WEBPACK_IMPORTED_MODULE_1___default.a.Tunnel));



/***/ }),

/***/ "./src/environments/environment.ts":
/*!*****************************************!*\
  !*** ./src/environments/environment.ts ***!
  \*****************************************/
/*! exports provided: environment */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "environment", function() { return environment; });
// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.
var environment = {
    production: false
};
/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.


/***/ }),

/***/ "./src/main.ts":
/*!*********************!*\
  !*** ./src/main.ts ***!
  \*********************/
/*! no exports provided */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _angular_platform_browser_dynamic__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/platform-browser-dynamic */ "./node_modules/@angular/platform-browser-dynamic/fesm5/platform-browser-dynamic.js");
/* harmony import */ var _app_app_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./app/app.module */ "./src/app/app.module.ts");
/* harmony import */ var _environments_environment__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./environments/environment */ "./src/environments/environment.ts");




if (_environments_environment__WEBPACK_IMPORTED_MODULE_3__["environment"].production) {
    Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["enableProdMode"])();
}
Object(_angular_platform_browser_dynamic__WEBPACK_IMPORTED_MODULE_1__["platformBrowserDynamic"])().bootstrapModule(_app_app_module__WEBPACK_IMPORTED_MODULE_2__["AppModule"])
    .catch(function (err) { return console.error(err); });


/***/ }),

/***/ 0:
/*!***************************!*\
  !*** multi ./src/main.ts ***!
  \***************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__(/*! C:\Users\gerja\Desktop\guacamole-angular\src\main.ts */"./src/main.ts");


/***/ })

},[[0,"runtime","vendor"]]]);
//# sourceMappingURL=main.js.map