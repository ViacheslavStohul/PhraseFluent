import axios from "axios";
import { Language } from "../interfaces/lang";
import { mapObjectToParams } from "../functions/params";
import { Test } from "../interfaces/test";

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

export const getList = async (request:ListRequest): Promise<Test[]> => {
  const { data } = await axios.get(`/list?${mapObjectToParams(request)}`);
  return data;
}

interface createTestRequest {
  title: string,
  description?: string,
  imageUrl?: string,
  languageUuid: string
}

export const createTest = async (request: createTestRequest): Promise<Test> => {
  const { data } = await axios.post(`/new`, request);
  return data;
}
