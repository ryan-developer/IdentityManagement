export default class UserIdentity {
    tenantID: string 
    identitySource: string 
    identityProvider: string 
    externalUserId: string 
    id: string 
    normalizedUserName: string 
    normalizedEmail: string 
    emailConfirmed: false
    securityStamp: string 
    concurrencyStamp: string 
    phoneNumber: string 
    phoneNumberConfirmed: false
    twoFactorEnabled: false
    lockoutEnd: string 
    lockoutEnabled: true
    accessFailedCount: number
}