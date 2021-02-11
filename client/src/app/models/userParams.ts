import { User } from './user';
/* to simplify passing argument to MembersService, we will pass this obj
*/
export class UserParams {
    city = 'ontario';
    gender: string;
    minAge = 19;
    maxAge = 99;
    role: string;
    pageNumber = 1;
    pageSize = 5;
    orderBy = 'lastActive';

    constructor(user: User) {
        this.gender = user.gender === 'female' ? 'male' : 'female';
    }
}
