import { defineStore } from "pinia";
import { IEntry } from "../models/entry";
import { IStatus } from "../models/status";
import { ref, Ref } from "vue";
import { Axios } from "axios";

export interface IEntryStore {
    initialise():Promise<void>;
    injectEntry(entry:IEntry, numberOfEntries:number):Promise<void>;
    populate():Promise<void>;
    getStatus():Promise<IStatus>;
    getWinners(numberOfEntries:number): Promise<IEntry[]> 
    hasWinners:Ref<boolean>;
    status: Ref<IStatus|undefined>;
    winningEntries: Ref<IEntry[]>;
}

export const createEntryStore = defineStore("entry-store", () : IEntryStore => {
    const axios = new Axios({
        baseURL : "http://localhost:5011/api"
    });

    const status = ref<IStatus>();
    const winningEntries = ref(new Array<IEntry>());
    const hasWinners = ref(false);
    async function populate():Promise<void> {
        const response = await axios.get("populate");
        status.value = response.data;
    }

    async function initialise():Promise<void> {
        if(status.value == undefined 
            || !status.value?.isLoaded
            || !status.value?.isPopulated) {
            await getStatus()
        }

        if(!status.value?.isLoaded) {
            const response = await axios.get("/");
            status.value = response.data;
        }

        if(!status.value?.isPopulated) {
            await populate();
        }
    }

    async function getWinners(numberOfEntries:number): Promise<IEntry[]> {
        const winnersRaw = (await axios.get("pick", {
            params: {
                numberOfEntries
            }
        })).data;

        const winners = JSON.parse(winnersRaw);
        winningEntries.value.length = 0;

        for(let winner of winners){
            winningEntries.value.push(winner);
        }
        return winners;
    }

    async function getStatus():Promise<IStatus> {
        const response = await axios.get("status");
        
        return status.value = JSON.parse(response.data);
    }

    async function injectEntry(entry:IEntry, numberOfEntries:number):Promise<void> {
        const formData = new FormData();

        formData.append("numberOfEntries", numberOfEntries.toString());

        if(entry.city != undefined)
        {
            formData.append("city", entry.city);
        }

        if(entry.email != undefined)
        {
            formData.append("email", entry.email);
        }

        if(entry.name != undefined)
        {
            formData.append("name", entry.name);
        }

        const response = await axios.postForm("inject", formData);

        status.value = JSON.parse(response.data);
    }

    return {
        initialise,
        injectEntry,
        getStatus,
        getWinners,
        hasWinners,
        populate,
        status,
        winningEntries
    }
})