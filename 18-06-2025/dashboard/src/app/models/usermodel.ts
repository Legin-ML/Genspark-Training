export class UserModel{
    constructor(
        public id : number = 0,
        public firstName : string,
        public lastName : string,
        public age : number,
        public gender : string,
        public state : string,
        public role : string

    ) {}
}