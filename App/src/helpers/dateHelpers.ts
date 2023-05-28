import Dayjs from "dayjs";

export class DateHelper {
    
    static format(date:Date | undefined, format:string):string {
        if(date == undefined) {
            return "Invalid";
        }

        return Dayjs(date).format(format);
    }
}