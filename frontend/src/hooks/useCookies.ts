// useCookies.ts
import { useState } from "react";
import { setCookie, getCookie, removeCookie } from "../utils/cookiesUtils";

const useCookies = () => {
  const set = (name: string, value: string, days: number) => {
    setCookie(name, value, days);
  };

  const get = (name: string): string | null => {
    return getCookie(name);
  };

  const remove = (name: string) => {
    removeCookie(name);
  };

  return {
    set,
    get,
    remove,
  };
};

export default useCookies;
