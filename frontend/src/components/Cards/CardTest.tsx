import React from "react";

const Card = () => {
  return (
    <nav className="bg-white shadow">
      <div className="max-w-7xl mx-auto px-2 sm:px-6 lg:px-8">
        <div className="relative flex items-center justify-between h-16">
          <div className="flex-1 flex items-center justify-center sm:items-stretch sm:justify-start">
            <div className="flex-shrink-0 flex items-center">
              <img
                className="block lg:hidden h-8 w-auto"
                src="https://placehold.co/100x100"
                alt="Airbnb logo for mobile"
              />
              <img
                className="hidden lg:block h-8 w-auto"
                src="https://placehold.co/100x100"
                alt="Airbnb logo for desktop"
              />
            </div>
            <div className="hidden sm:block sm:ml-6">
              <div className="flex space-x-4">
                <a
                  href="#"
                  className="text-gray-700 px-3 py-2 rounded-md text-sm font-medium"
                  aria-current="page"
                >
                  Gdziekolwiek
                </a>
                <a
                  href="#"
                  className="text-gray-700 px-3 py-2 rounded-md text-sm font-medium"
                >
                  Dowolny tydzień
                </a>
                <a
                  href="#"
                  className="text-gray-700 px-3 py-2 rounded-md text-sm font-medium"
                >
                  Dodaj gości
                </a>
                <button className="p-2 rounded-full bg-red-500 text-white">
                  <i className="fas fa-search"></i>
                </button>
              </div>
            </div>
          </div>
          <div className="absolute inset-y-0 right-0 flex items-center pr-2 sm:static sm:inset-auto sm:ml-6 sm:pr-0">
            <a
              href="#"
              className="text-gray-700 px-3 py-2 rounded-md text-sm font-medium"
            >
              Włącz tryb gospodarza
            </a>
            <button className="bg-gray-100 p-2 rounded-full text-gray-500 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500">
              <i className="fas fa-bars"></i>
            </button>
            <button className="ml-3 bg-gray-100 p-2 rounded-full text-gray-500 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500">
              <i className="far fa-heart"></i>
            </button>
            <div className="ml-3 relative">
              <div className="bg-gray-100 p-2 rounded-full text-gray-500 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500">
                <i className="far fa-user"></i>
              </div>
            </div>
          </div>
        </div>
      </div>
    </nav>
  );
};

export default Card;
