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

    static convert(entry:IEntry): Entry {
        const newEntry = new Entry();
        newEntry.city = entry.city;
        newEntry.email = entry.email;
        newEntry.id = entry.id;
        newEntry.isFlagged = entry.isFlagged;
        newEntry.name = entry.name;
        return newEntry;
    }
}