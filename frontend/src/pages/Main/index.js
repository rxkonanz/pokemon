import React, { useState, useEffect } from 'react'

import { Container, Dashboard, Presentation, Header, Board } from './styles'
import Search from './components/Search'
import Pokemon from './components/Pokemon'
import Pagination from './components/Pagination'

import { api } from '../../services/api'

const Main = () => {
  const [pokemons, setPokemons] = useState({})
  const [pokemon, setPokemon] = useState({})
  const [pokemonSearch, setPokemonSearch] = useState("")
  const [page, setPage] = useState(1)
  const [quantity,] = useState(9)
  const [totalPages, setTotalPages] = useState(0)
  const [isFound, setIsFound] = useState(true)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const fetchPokemons = async () => {
      setLoading(true)

      const response = await api.get('pokemon', {
        params: {
          name: pokemonSearch,
          page,
          quantity
        }
      })

      const { data } = response.data
      setTotalPages(response.data.totalPages)
      setPokemons(data)
      setPokemon(data[0] || {})

      const found = response.data.data.length > 0
      setIsFound(found)

      if (page > totalPages)
        setPage(1)

      setLoading(false)
    }

    fetchPokemons()
  }, [page, pokemonSearch, quantity, totalPages])

  return (
    <Container>
      <Dashboard>
        <Presentation>
          <Header>
            <h1>Hi Igor,</h1>
            <p>Let's search an amazing <span>Pokemon</span></p>
          </Header>

          <Search
            pokemons={pokemons}
            pokemon={pokemon}
            setPokemon={setPokemon}
            pokemonSearch={pokemonSearch}
            setPokemonSearch={setPokemonSearch}
          />
          <Pagination
            page={page}
            setPage={setPage}
            totalPages={totalPages}
          />
        </Presentation>

        <Board>
          <Pokemon
            pokemon={pokemon}
            isFound={isFound}
            loading={loading}
          />
        </Board>
      </Dashboard>
    </Container>
  )
}

export default Main
