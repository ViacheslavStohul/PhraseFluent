import axios from "axios";
import { Language } from "../interfaces/lang";
import { mapObjectToParams } from "../functions/params";
import { Test, createTestRequest } from "../interfaces/test";
import { List } from "../interfaces/list";

const baseUrl = '/api/word'

export const getLangs = async (): Promise<Language[]> => {
    const { data } = await   axios.get(`${baseUrl}/languages`);
    return data;
}

export interface ListRequest {
  Page: number;
  Size: number;
  Language?: string;
  Username?:string;
  Title?: string;
}

export const getList = async (request:ListRequest): Promise<List<Test>> => {
  const { data } = await axios.get(`/list?${mapObjectToParams(request)}`);
  return data;
}

export const createTest = async (request: createTestRequest): Promise<Test> => {
  const { data } = await axios.post(`/new`, request);
  return data;
}
