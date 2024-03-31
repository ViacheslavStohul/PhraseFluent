import axios from "axios";
import { Language } from "../components/interfaces/lang";

const baseUrl = '/api/word'

export const getLangs = async (): Promise<Language[]> => 
  axios.get(`${baseUrl}/languages`);
