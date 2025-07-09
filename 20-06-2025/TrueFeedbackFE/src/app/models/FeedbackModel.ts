export class FeedbackModel {
  constructor(
    public id?: number,
    public userId: string = '',
    public message: string = '',
    public rating: number = 5,
    public reply?: string,
    public replyId?: string,
    public created?: Date,
    public username?: string
  ) {}
}
