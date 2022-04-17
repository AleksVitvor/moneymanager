"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.UserManagementRoutes = void 0;
var user_role_guard_1 = require("../../shared/guards/user-role.guard");
var user_management_component_1 = require("./user-control-component/user-management.component");
exports.UserManagementRoutes = [
    {
        path: '',
        component: user_management_component_1.UserManagementComponent,
        canActivate: [user_role_guard_1.UserRoleGuard],
        data: { title: 'User Management', roles: 'Admin' }
    }
];
//# sourceMappingURL=user-management.routing.js.map