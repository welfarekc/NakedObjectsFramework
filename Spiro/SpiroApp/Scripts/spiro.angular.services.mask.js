/// <reference path="typings/underscore/underscore.d.ts" />
/// <reference path="spiro.models.ts" />
/// <reference path="spiro.angular.viewmodels.ts" />
/// <reference path="spiro.angular.app.ts" />
/// <reference path="spiro.angular.config.ts" />
var Spiro;
(function (Spiro) {
    (function (Angular) {
        Spiro.Angular.app.service('mask', function () {
            var color = this;

            color.toLocalFilter = function (remoteMask) {
                if (Spiro.Angular.maskMap) {
                    return Spiro.Angular.maskMap[remoteMask];
                }

                return null;
            };
        });
    })(Spiro.Angular || (Spiro.Angular = {}));
    var Angular = Spiro.Angular;
})(Spiro || (Spiro = {}));
//# sourceMappingURL=spiro.angular.services.mask.js.map
