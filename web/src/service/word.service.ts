import axios from "axios";
import { Language } from "../components/interfaces/lang";

const baseUrl = '/api/word'

export const getLangs = async (): Promise<Language[]> => {
  try {
    const { data } = await   axios.get(`${baseUrl}/languages`);
    return data;
  } catch (error) {
    throw error;
  }
}
