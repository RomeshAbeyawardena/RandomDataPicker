export interface IPersistedEntry {
   id?:string;
   cId:number;
   name?:string;
   email?:string;
   city?:string;
   isFlagged:boolean;
   created?:Date;
}

export class PersistedEntry implements IPersistedEntry {
    id?: string;
    cId: number;
    name?: string;
    email?: string;
    city?: string;
    isFlagged: boolean;
    created?: Date;
    
    constructor() {
        this.cId = 0;
        this.isFlagged = false;
    }

    static convert(entry: IPersistedEntry): PersistedEntry {
        const newEntry = new PersistedEntry();
        newEntry.id = entry.id;
        newEntry.cId = entry.cId;
        newEntry.city = entry.city;
        newEntry.created = entry.created;
        newEntry.email = entry.email;
        newEntry.isFlagged = entry.isFlagged;
        newEntry.name = entry.name;
        return newEntry;
    }
}