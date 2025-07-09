export class SignUpModel {
    constructor (
        userName: string,
        public email: string,
        public password: string,
        roleName: string
    ) {}
}