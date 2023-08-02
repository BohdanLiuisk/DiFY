export interface User {
    id: number;
    firstName: string;
    lastName: string;
    name: string;
    login: string;
    email: string;
    createdOn: Date;
    online: boolean;
    avatarUrl: string;
    isCurrentUser: boolean;
}
