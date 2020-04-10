export class UserProfile
{
    id: string;
    email: string;
    firstName: string;
    lastName: string;
    hasLoggedIn: boolean;
    userPermissions: any[];
}

export class SecurityContext
{
    roles: string[];
    userProfile: UserProfile;

    userIsAdmin(): boolean {
        return !!this.roles && !!this.roles.find(c => c === "Admin");
    }

    userIsPremium(): boolean {
        return !!this.roles && !!this.roles.find(c => c === "Premium");
    }
}