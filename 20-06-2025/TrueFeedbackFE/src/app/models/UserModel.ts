
export class UserModel{

    constructor(
        id: string,
        public userName: string,
        public email: string,
        public roleName : string,
    ){}

    static fromApi(data: any): UserModel {
    return new UserModel(
      data.id,
      data.userName,
      data.email,
      data.role?.roleName ?? ''
    );
  }
}