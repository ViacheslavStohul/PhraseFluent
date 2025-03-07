import axios from "axios";
import { mapObjectToParams } from "../functions/params";
import { BeginTestResponse, ICard, NextTestRequest, Test, createTestRequest } from "../interfaces/test";
import { List } from "../interfaces/list";

export interface ListRequest {
  Page: number;
  Size: number;
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

export const createCard = async (request: ICard): Promise<ICard> => {
  const { data } = await axios.post(`/card/new`, request);
  return data;
}

export const beginTest = async (id: string): Promise<BeginTestResponse> => {
  const { data } = await axios.post(`/begin?testUuid=${id}`);
  return data;
}
export const statsTest = async (id: string, optionId?: string): Promise<Test> => {
  const { data } = await axios.get(`/statistics?testUuid=${id}${optionId ? '&answerOptionUuid=' + optionId : ''}`);
  return data;
}

export const statsExcel = async (id: string): Promise<void> => {
  try {
    const response = await axios.get(`/statistics/excel?testUuid=${id}`, {
      responseType: "blob",
    });

    const url = window.URL.createObjectURL(new Blob([response.data]));
    const link = document.createElement("a");
    link.href = url;
    link.setAttribute("download", `statistics_${id}.xlsx`);
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    window.URL.revokeObjectURL(url);
  } catch (error) {
    console.error("Error downloading the file:", error);
  }
}


export const nextTest = async (next: NextTestRequest): Promise<BeginTestResponse> => {
  const { data } = await axios.post(`/next`, next);
  return data;
}

export const getTest = async (id: string): Promise<Test> => {
  const { data } = await axios.get(`api/test/?uuid=${id}`);
  return data;
}
