export interface IEntry {
    id:number;
    name?:string;
    email?:string;
    city?:string;
    isFlagged:boolean;
}

export class Entry implements IEntry {
    id:number;
    name?:string;
    email?:string;
    city?:string;
    isFlagged:boolean;
    
    constructor() {
        this.id = 0;
        this.isFlagged = false;
    }
}