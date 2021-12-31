export interface Message {
    id: number;
    senderId: number;
    sendUserName: string;
    senderPhotoUrl: string;
    recepientId: number;
    recepientUserName: string;
    recepientPhotoUrl: string;
    content: string;
    dateRead?: Date;
    messageSent: Date;
}