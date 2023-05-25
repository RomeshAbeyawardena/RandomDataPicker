import { defineStore } from "pinia";
import { IEntry } from "../models/entry";
import { IStatus } from "../models/status";
import { ref, Ref } from "vue";
import { Axios } from "axios";

export interface IEntryStore {
    initialise():Promise<void>;
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
        var rawStatus = JSON.parse(response.data);
        
        return status.value = rawStatus.result;
    }

    return {
        initialise,
        getStatus,
        getWinners,
        hasWinners,
        populate,
        status,
        winningEntries
    }
})