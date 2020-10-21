export interface User {
    id: string;
    userName: string;
    email: string;
    firstName: string;
    lastName: string;
    tenantId: string;
    active: boolean;
    // password: string;
    jwtToken: string;
    refreshToken: string;
}
